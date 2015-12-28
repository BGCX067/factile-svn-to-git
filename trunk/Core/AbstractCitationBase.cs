using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Factile.Core
{
    public abstract class AbstractCitationBase<T> : EntityBase<T>
    {
        [BelongsTo(Column = "FactID")]
        public Fact Fact { get; set; }

        [Property]
        public DateTime PublishDate { get; set; }

        [Property(Length = 100)]
        public string Author { get; set; }
    }
}
