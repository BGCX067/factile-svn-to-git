using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Factile.Core.DataInterfaces
{
    public interface IDao<T, IdT>
    {
        T CreateNew();
        T GetById(IdT id);
        IList<T> GetAll();
        T GetFirst();
        IList<T> GetFirst(int n);
        T Save(T entity);
        void Update(T entity);
        T SaveOrUpdate(T entity);
        T SaveOrUpdateCopy(T entity);
        void Delete(T entity);
        void CommitChanges();
    }
}
