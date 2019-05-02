using App.Library.CodeStructures.Behavioral;
using App.Library.EntityFramework.Configuration;
using App.Library.EntityFramework.Contexts.ConnectionStringProviders;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace App.Library.EntityFramework.Contexts {

    /// <summary>
    ///     A <see cref="DbContext"/> that requires a connection string
    /// provider and a <see cref="DbModelBuilder"> configuration.  
    /// </summary>
    public class CompositeContext 
                    : DbContext {

        /// <summary>
        ///     Constructor accepts the configuration used to map the entities to a database schema
        /// and a provider that supplies the connection string
        /// </summary>
        /// <param name="the_model_configuration">
        ///     Configuration that is applied to a <see cref="DbModelBuilder"/>.  This builds the is the 
        /// map between entity framework the underlying database schema.
        /// </param>
        /// <param name="connection_string_provider">
        ///     Provider that is used to get the connection string for the context
        /// </param>
        public CompositeContext 
                       ( IModelConfiguration the_model_configuration 
                       , IConnectionStringProvider connection_string_provider ) 
                : base ( connection_string_provider.connection_string ) {
            
            model_configuration = Guard.IsNotNull( the_model_configuration, "the_model_configuration" );            
        }

        

        protected override void OnModelCreating 
                                    ( DbModelBuilder model_builder ) {

            //Added this bit, not sure if it is the right thing, but brought it across from the old app
            model_builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            model_configuration.configure(  model_builder );
            base.OnModelCreating( model_builder );
        }

        // data model builder configuration.
        private readonly IModelConfiguration model_configuration;        
    }

}