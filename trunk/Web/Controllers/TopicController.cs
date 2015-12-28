using System;
using Factile.Core;
using Castle.MonoRail.Framework;

namespace Factile.Web.Controllers
{
    public class TopicController  : SmartDispatcherController
    {
        [Layout("default"), Rescue("generalerror")]
        public void List()
        {                            
            this.PropertyBag["facts"] = Fact.FindAll();
        }
        
    }

    public class FactController : SmartDispatcherController
    {
        [Layout("default"), Rescue("generalerror")]
        public void Add()
        {
            
        }

        [Layout("default"), Rescue("generalerror")]
        public void Save([DataBind("Fact")] Fact fact, [DataBind("Topic")] Topic formTopic)
        {
            Topic topic = Topic.FindByName(formTopic.Name);
            if (topic == null)
            {
                topic = formTopic;
                topic.SaveAndFlush();
            }

            fact.Topic = topic;            
            fact.Save();

            PropertyBag["Fact"] = fact;
        }
    }
}
