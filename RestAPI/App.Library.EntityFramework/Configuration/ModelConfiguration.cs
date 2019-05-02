using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace App.Library.EntityFramework.Configuration {

    public abstract class ModelConfiguration<E> 
                : EntityTypeConfiguration<E>
                , IModelConfiguration 
        where E : class {


        public void configure ( DbModelBuilder target_to_configure ) {
            
            target_to_configure.Configurations.Add( this );
        }
    }
}