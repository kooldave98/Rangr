

using App.Library.EntityFramework.Configuration;

namespace App.Persistence.EF.Domain.Main
{
    public class ModelBuilder : ModelConfigurationRegister
    {

        public ModelBuilder(string schema)
        {
            register(new Users.ModelBuilder(schema));
            register(new Posts.ModelBuilder(schema));
            register(new GeoLocations.ModelBuilder(schema));
            register(new Connections.ModelBuilder(schema));
            register(new HashTags.ModelBuilder(schema));
            register(new HashTaggedPosts.ModelBuilder(schema));
        }

    }
}
