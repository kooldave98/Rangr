using System.Data.Entity;
using App.Library.CodeStructures.Structural;

namespace App.Library.EntityFramework.Configuration {

    public interface IModelConfiguration : IConfiguration<DbModelBuilder> { }

}