using System.Data.Entity;
using App.Persistence.EF.Infrastructure;
using App.Persistence.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace App.Persistence.EF
{
    public class EntityDbContext : DbContext
    {

        public EntityDbContext()
        {
            InitialiseDatabase();
        }

        public EntityDbContext(string connectionStringOrName)
            //: base(connectionStringOrName)//This will be passed in manually.
            : base("name=AppDBConnection")//This will pick up from the config file from the executing client app.
        {
            InitialiseDatabase();
        }

        public void InitialiseDatabase()
        {
            // Initialise the database.
            Database.SetInitializer(new EntityDbContextInitializer());
            Database.Initialize(false);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // mappings if model is not same as database schema
            //var mapUser = modelBuilder.Entity<User>();
            //mapUser.Map(m => m.ToTable("User"));


            //modelBuilder.Entity<Post>()
            //    .HasRequired(d => d.User)
            //    .WithMany(d => d.Posts)
            //    .HasForeignKey(d => d.UserID)
            //    .WillCascadeOnDelete(false);


            //modelBuilder.Entity<CommentLike>()
            //   .HasRequired(d => d.User)
            //   .WithMany(d => d.Posts)
            //   .HasForeignKey(d => d.UserID)
            //   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<App>()
            //    .HasRequired(d => d.Developer)
            //    .WithMany(d => d.Apps)
            //    .HasForeignKey(d => d.DeveloperID)
            //    .WillCascadeOnDelete(false);     


            base.OnModelCreating(modelBuilder);
        }
    }

}