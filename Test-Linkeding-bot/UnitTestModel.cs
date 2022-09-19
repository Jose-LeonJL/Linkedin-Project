using NUnit.Framework;
using Linkedin_bot.models;
using Linkedin_bot.db;

namespace Test_Linkeding_bot
{
    public class Tests_models
    {
        database database;
        [SetUp]
        public void Setup()
        {
            database = database.getdatabase();
        }

        [Test]
        public void test_select_proxys()
        {
            
            
            Assert.Pass();
        }
    }
}