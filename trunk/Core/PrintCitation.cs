using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Factile.Core
{
    [ActiveRecord]
    public class PrintCitation : AbstractCitationBase<PrintCitation>, ICitation
    {
        [Property]
        public int PageNumber { get; set; }

        [Property]
        public string BookTitle { get; set; }

        public void LoadFromXml(string XmlBody)
        {
            throw new NotImplementedException();
        }

        public string HtmlString()
        {
            throw new NotImplementedException();
        }
    }
}
