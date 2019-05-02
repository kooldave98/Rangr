

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;

namespace App.Persistence.EF.Domain.Main.Users
{
    public class ModelBuilder : ModelConfiguration<User>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("User", schema));

            //Worksuite stuff
            //HasMany(m => m.Address)
            //    .WithRequired();            

            //Property(u => u.dateofbirth).HasColumnType("datetime2");


            //Old GeoNow app stuff that wasn't actually used
            //so may wanna refrain from using this
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



            


        }
    }
}
