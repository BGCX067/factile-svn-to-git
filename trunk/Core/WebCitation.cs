using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Factile.Core
{
    [ActiveRecord]
    public class WebCitation : AbstractCitationBase<WebCitation>, ICitation
    {
        [Property]
        public string Url { get; set; }

        [Property]
        public string Title { get; set; }

        public void LoadFromXml(string XmlBody)
        {
            throw new NotImplementedException();
        }

        public string HtmlString()
        {
            return String.Format("<a href=\"{0}\" class=\"fact\">{1}</a>", Url, Title);
        }
    }
}
