using System.Data.Entity;
using App.Library.CodeStructures.Structural;

namespace App.Library.EntityFramework.Configuration {

    public class ModelConfigurationRegister : ConfigurationRegister<DbModelBuilder,IModelConfiguration>, IModelConfiguration { }

}