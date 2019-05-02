using App.Library.EntityFramework.Contexts;
using System.Data.Entity;

namespace App.Library.EntityFramework.Initialization
{
    public class SimpleInitializer : DropCreateDatabaseIfModelChanges<CompositeContext>
    {

        public SimpleInitializer()
        {
        }


        protected override void Seed(CompositeContext context)
        {
            //SeedUser(context);
        }


        //// create default admin users
        //private void SeedUser(EntityDbContext context)
        //{
        //    var users = new List<User>
        //    {
        //        new User 
        //        {
        //             DisplayName = "Seeded User"       
        //        }
        //    };

        //    users.ForEach(u => context.Users.Add(u));
        //    context.SaveChanges();
        //}
    }

}
