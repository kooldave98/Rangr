

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;
namespace App.Persistence.EF.Domain.Main.Posts
{
    public class ModelBuilder : ModelConfiguration<Post>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("Post", schema));
        }
    }
}
