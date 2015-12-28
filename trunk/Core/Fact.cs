using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;


namespace Factile.Core
{

    [ActiveRecord]
    public class Topic: EntityBase<Topic>
    {
        [Property]
        public string Name { get; set; }

        [HasMany(typeof(Fact))]
        public IList<Fact> Facts { get; set; }
        
        public Topic()
        {            
        }

        public static Topic FindByName(string name)
        {
            return Topic.FindFirst(new EqExpression("Name", name));
        }
    }
    
    [ActiveRecord]
    public class Fact : EntityBase<Fact>
    {
        [Property(Length=1000)]
        public string Title { get; set; }

        [Property(ColumnType="StringClob", Length=9000)]
        public string Body { get; set; }

        [HasMany(typeof(PrintCitation))]
        public IList<PrintCitation> PrintCitations { get; set; }

        [HasMany(typeof(WebCitation))]
        public IList<WebCitation> WebCitations { get; set; }      

        public List<ICitation> Citations
        {
            get
            {
                List<ICitation> rv = new List<ICitation>();

                foreach (ICitation citation in PrintCitations)                
                    rv.Add(citation);                         
           
                foreach (ICitation citation in WebCitations)                
                    rv.Add(citation);
                
                return rv;
            }
        }

        [BelongsTo(Column = "TopicID")]
        public Topic Topic { get; set; }

        public static Fact GetByTitle(string title)
        {
            return FindFirst(new EqExpression("Title", title));            
        }
    }    
}
