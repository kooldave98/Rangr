

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;

namespace App.Persistence.EF.Domain.Main.HashTags
{
    public class ModelBuilder : ModelConfiguration<HashTag>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("HashTag", schema));
        }
    }
}
