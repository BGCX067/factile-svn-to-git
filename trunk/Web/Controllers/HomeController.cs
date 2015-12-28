using System;
using Castle.MonoRail.Framework;

namespace Factile.Web.Controllers
{
    

    [Layout("default"), Rescue("generalerror")]
    public class HomeController : SmartDispatcherController
    {
        public void Index()
        {
            PropertyBag["name"] = "John Doe";	
        }
    }
}
