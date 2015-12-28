using System;
using System.Collections.Generic;
using System.Text;
using Factile.Core.DataInterfaces;
using NHibernate;
using NHibernate.Expression;
using Factile.Core;
using NHibernate.Cache;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate.Cfg;

namespace Factile.Dao
{
    public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT>
    {
        internal Type concreteType;

        public AbstractNHibernateDao()
        {
        }

        public AbstractNHibernateDao(Type returnType)
        {
            concreteType = returnType;
        }

        virtual public T CreateNew()
        {
            return (T)concreteType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
        }

        /// <summary>
        /// Loads an instance of type T from the DB based on its ID.
        /// </summary>
        virtual public T GetById(IdT id)
        {
            return (T)NHibernateSession.Load(concreteType, id);
        }

        /// <summary>
        /// Loads every instance of the requested type with no filtering.
        /// </summary>
        virtual public IList<T> GetAll()
        {
            return GetByCriteria();
        }

        /// <summary>
        /// Loads the first instance of the requested type with no filtering
        /// (this is useful for testing)
        /// </summary>
        /// <returns></returns>
        virtual public T GetFirst()
        {
            IList<T> first = this._GetFirstN(1);

            if (first.Count == 1)
            {
                return first[0];
            }

            return default(T);
        }

        virtual public IList<T> GetFirst(int n)
        {
            IList<T> first = this._GetFirstN(n);

            if (first.Count == n)
            {
                return first;
            }

            return new List<T>();
        }

        protected IList<T> _GetFirstN(int n)
        {
            return NHibernateSession.CreateCriteria(concreteType)
               .SetFirstResult(0)
               .SetMaxResults(n)
               .List<T>() as List<T>;
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        virtual public IList<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(concreteType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }

            return criteria.List<T>() as List<T>;
        }


        /// <summary>
        /// For entities that have assigned ID's, you must explicitly call Save to add a new one.
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        virtual public T Save(T entity)
        {
            NHibernateSession.Save(entity);
            return entity;
        }

        /// <summary>
        /// For entities with automatatically generated IDs, such as identity, SaveOrUpdate may 
        /// be called when saving a new entity.  SaveOrUpdate can also be called to _update_ any 
        /// entity, even if its ID is assigned.
        /// </summary>
        virtual public T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            return entity;
        }

        virtual public T SaveOrUpdateCopy(T entity)
        {
            NHibernateSession.SaveOrUpdateCopy(entity);
            return entity;
        }

        virtual public void Update(T entity)
        {
            NHibernateSession.Update(entity);
        }

        virtual public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
        }

        /// <summary>
        /// Commits changes regardless of whether there's an open transaction or not
        /// </summary>
        virtual public void CommitChanges()
        {
            NHibernateSessionManager.Instance.GetSession().Flush();
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        protected ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSession();
            }
        }


        #region IDao<T,IdT> Members

        T IDao<T, IdT>.CreateNew()
        {
            throw new NotImplementedException();
        }

        T IDao<T, IdT>.GetById(IdT id)
        {
            throw new NotImplementedException();
        }

        IList<T> IDao<T, IdT>.GetAll()
        {
            throw new NotImplementedException();
        }

        T IDao<T, IdT>.GetFirst()
        {
            throw new NotImplementedException();
        }

        IList<T> IDao<T, IdT>.GetFirst(int n)
        {
            throw new NotImplementedException();
        }

        T IDao<T, IdT>.Save(T entity)
        {
            throw new NotImplementedException();
        }

        void IDao<T, IdT>.Update(T entity)
        {
            throw new NotImplementedException();
        }

        T IDao<T, IdT>.SaveOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        T IDao<T, IdT>.SaveOrUpdateCopy(T entity)
        {
            throw new NotImplementedException();
        }

        void IDao<T, IdT>.Delete(T entity)
        {
            throw new NotImplementedException();
        }

        void IDao<T, IdT>.CommitChanges()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {
        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private NHibernateSessionManager()
        {
            InitSessionFactory();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }

        #endregion

        private void InitSessionFactory()
        {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor)
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSession(interceptor);
        }

        public ISession GetSession()
        {
            return GetSession(null);
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor)
        {
            ISession session = ContextSession;

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = sessionFactory.OpenSession(interceptor);
                }
                else
                {
                    session = sessionFactory.OpenSession();
                }

                ContextSession = session;
            }


            return session;
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSession()
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen)
            {
                session.Flush();
                session.Close();
            }

            ContextSession = null;
        }

        public void BeginTransaction()
        {
            ITransaction transaction = ContextTransaction;

            if (transaction == null)
            {
                transaction = GetSession().BeginTransaction();
                ContextTransaction = transaction;
            }
        }

        public void CommitTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction()
        {
            ITransaction transaction = ContextTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ITransaction ContextTransaction
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ITransaction)System.Web.HttpContext.Current.Items[TRANSACTION_KEY];
                }
                else
                {
                    return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
                }
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[TRANSACTION_KEY] = value;
                }
                else
                {
                    CallContext.SetData(TRANSACTION_KEY, value);
                }
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ISession ContextSession
        {
            get
            {
                if (IsInWebContext())
                {
                    //return (ISession)HttpContext.Current.Session[SESSION_KEY];
                    return (ISession)HttpContext.Current.Items[SESSION_KEY];
                }
                else
                {
                    return (ISession)CallContext.GetData(SESSION_KEY);
                }
            }
            set
            {
                if (IsInWebContext())
                {
                    //HttpContext.Current.Session[SESSION_KEY] = value;
                    HttpContext.Current.Items[SESSION_KEY] = value;
                }
                else
                {
                    CallContext.SetData(SESSION_KEY, value);
                }
            }
        }

        private bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private ISessionFactory sessionFactory;
    }
}
