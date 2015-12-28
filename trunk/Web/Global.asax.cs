using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Factile.Core;

namespace Factile.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ActiveRecordStarter.Initialize(ActiveRecordSectionHandler.Instance,
                new Type[] { typeof(Fact), typeof(WebCitation), typeof(PrintCitation), typeof(Topic) });

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}