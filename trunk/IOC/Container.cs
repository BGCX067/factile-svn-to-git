using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Windsor.Configuration.Interpreters;
using System.Reflection;
using System.IO;
using Castle.Windsor;

namespace Factile.IOC
{        

    public static class Container
    {
        private static WindsorContainer _Container;
        public static T Resolve<T>()
        {
            if (_Container == null)
                _Initialize();

            return _Container.Resolve<T>();
        }

        public static T Resolve<T>(string key)
        {
            if (_Container == null)
                _Initialize();

            return _Container.Resolve<T>(key);
        }

        private static void _Initialize()
        {
            _Container = new WindsorContainer(String.Format("{0}\\bin\\Windsor.config", AppDomain.CurrentDomain.BaseDirectory));

            //            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CostingEngine.IOC.Windsor.boo"))
            //            {
            //                BooReader.Read(_Container, stream, "Windsor");	
            //            }                                    
        }
    }
}
