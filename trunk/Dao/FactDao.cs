using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Factile.Core;

namespace Factile.Dao
{
    public class FactDao : AbstractNHibernateDao<IFact, int>
    {        
        public FactDao()
        {
            concreteType = typeof(Fact);
        }

        

        public IFact GetByTitle(string p)
        {
            throw new NotImplementedException();
        }
    }
}
