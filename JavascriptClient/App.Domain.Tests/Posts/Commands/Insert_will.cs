using App.Domain.Posts;
using App.Domain.Posts.Commands;
using App.Persistence.EF.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Spatial;
using System.Linq;

namespace App.Domain.Tests.Posts.Commands
{
    //[TestClass]
    public class Insert_will
    {
        private RepositorySet repositories;

        private Insert Command;
        private InsertRequest Request;

        //[TestInitialize]
        public void before_each_test()
        {
            repositories = new RepositorySet();
            Command = new Insert();
        }

        //[TestMethod]
        public void persist_a_post_in_the_database()
        {
            Request = new InsertRequest
            {
                GeoLocation = "",//DbGeography.FromText("POINT(-122.296623 47.640405)"),
                Text = "New Post",
                UserID = 1
            };

            //Act
            var post_id = Command.Execute(Request);

            //Assert
            var res = repositories.Posts.Entries.Where(p => p.ID == post_id.ID);
            Assert.AreEqual(true, res.Any());
        }
    }
}
