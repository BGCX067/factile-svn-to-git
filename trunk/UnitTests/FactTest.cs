using System;
using NUnit.Framework;
using Factile.Core;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;

namespace UnitTests
{
    public class AbstractTestBase
    {
        #region StartUp
        [TestFixtureSetUp]
        public void StartUp()
        {
            ActiveRecordStarter.Initialize(ActiveRecordSectionHandler.Instance,
                               new Type[] { typeof(Topic), typeof(Fact), typeof(WebCitation), typeof(PrintCitation) });

            // If you want to let ActiveRecord create the schema for you:
            ActiveRecordStarter.CreateSchema();

            Topic category = new Topic() { Name = "Earth" };
            category.SaveAndFlush();
            
            Fact newFact = new Fact() { Body = "The earth is round", Title = "Blah", Topic = category };            
            newFact.SaveAndFlush();

            WebCitation citation = new WebCitation()
            {
                Author = "ignu",
                PublishDate = new DateTime(2007, 1, 1),
                Title = "blah",
                Url = "http://www.yahoo.com",
                Fact = newFact
            };

            citation.SaveAndFlush();
        }
        #endregion

        public AbstractTestBase()
        {
            
        }
    }
    [TestFixture]
    public class FactTest : AbstractTestBase
    {

        [Test]
        public void CanPerformFactCRUD()
        {
            // TEST CREATE            
            Fact fact = new Fact { Body = "Test", Title = "New Fact" };
            fact.Save();            
            Assert.Greater(fact.ID, 0);

            Fact loadedFact = Fact.GetByTitle("New Fact");
            Assert.AreEqual(fact.ID, loadedFact.ID);

            loadedFact.DeleteAndFlush();                             
            loadedFact = Fact.GetByTitle("New Fact");
            Assert.IsNull(loadedFact);                        
        }
    }
}
