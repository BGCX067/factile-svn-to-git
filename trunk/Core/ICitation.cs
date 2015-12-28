using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Factile.Core
{
    public interface ICitation
    {
        DateTime PublishDate { get; set; }
        string Author { get; set; }
        void LoadFromXml(string XmlBody);
        string HtmlString();
    }
}
