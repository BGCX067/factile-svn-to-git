using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Factile.Core
{
    public class EntityBase<T> : ActiveRecordBase<T>
    {
        [PrimaryKey]
        public int ID { get; set; }

        public EntityBase() { }
    }
}
