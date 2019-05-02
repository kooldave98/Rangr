using App.Library.EntityFramework.Configuration;
using App.Library.EntityFramework.Contexts;
using App.Library.EntityFramework.Contexts.ConnectionStringProviders;
using System.Data.Entity;

namespace App.Persistence.EF.Infrastructure {

    /// <summary>
    ///     Worksuite configuration database context
    /// </summary>
    public class AppContext 
                    : CompositeContext {


        /// <inheritdoc/>
        public AppContext 
                       ( IModelConfiguration the_model_configuration 
                       , IConnectionStringProvider connection_string_provider ) 
                : base ( the_model_configuration
                       , connection_string_provider ) 
        {
            InitialiseDatabase();
        }

        private void InitialiseDatabase()
        {
            // Initialise the database.
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppContext>());
            Database.Initialize(false);
        }
    }
}