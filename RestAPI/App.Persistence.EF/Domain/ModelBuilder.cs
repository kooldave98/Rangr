

using App.Library.EntityFramework.Configuration;

namespace App.Persistence.EF.Domain
{
    public class ModelBuilder : ModelConfigurationRegister
    {

        public ModelBuilder()
        {
            register(new Main.ModelBuilder("Main"));
        }

    }
}
