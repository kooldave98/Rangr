

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;

namespace App.Persistence.EF.Domain.Main.HashTaggedPosts
{
    public class ModelBuilder : ModelConfiguration<HashTaggedPost>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("HashTaggedPost", schema));
        }
    }
}
