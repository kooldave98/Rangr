using App.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace App.Persistence.EF.Infrastructure
{

    /// <summary>
    /// Database "Reseed" when creating database.
    /// </summary>
    public class EntityDbContextInitializer : DropCreateDatabaseIfModelChanges<EntityDbContext>
    {

        public EntityDbContextInitializer()
        {
        }


        protected override void Seed(EntityDbContext context)
        {
            SeedUser(context);
        }


        // create default admin users
        private void SeedUser(EntityDbContext context)
        {
            var users = new List<User>
            {
                new User 
                {
                    Email = "me@example.com",
                    Password = "aa"                    
                }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }

}
