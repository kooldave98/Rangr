
using System;
using App.WebAPI.Infrastructure.UsageLogs;
using Ninject;
using Ninject.Activation;
using App.Persistence.EF.Infrastructure;
using App.Library.Ninject.Configuration;
using App.Integration.Configuration;

namespace App.WebAPI.Infrastructure.DependencyConfiguration
{

    public class AppWebApiConfiguration : IDependencyConfiguration  {

        /// <summary>
        ///     Load all the configurations in this assembly
        /// </summary>
        /// <param name="kernel">
        ///     The kernel to be configured
        /// </param>
        /// <param name="scope">
        ///     The scope to use for any scope sensitive configurations.
        /// </param>
        public void configure ( IKernel kernel, Func<IContext, object> scope ) {
            register_domain_configurations( kernel, scope );
            register_client_configurations( kernel, scope );
        }

        private void register_domain_configurations ( IKernel kernel, Func<IContext, object> scope ) {
            // configure withe the standard domain

            ServiceLayerDependencyConfiguration.full_domain( kernel, scope );
            persistence_configuration.configure(kernel, scope);         
        }

        private void register_client_configurations  ( IKernel kernel, Func<IContext, object> scope ) {

            //var library_configuration = new LibraryConfiguration();
            //library_configuration.configure( kernel, scope );

            //var domain_type_library_configuration = new DomainTypesLibraryConfiguration();
            //domain_type_library_configuration.configure( kernel, scope );

            //var route_configuration = new RouteConfiguration();
            //route_configuration.configure( kernel, scope );

            var usage_log_dependency_configuration = new UsageLogsDependencyConfiguration();
            usage_log_dependency_configuration.configure(kernel, scope);
        }

        private static readonly PersistenceEFConfiguration persistence_configuration = new PersistenceEFConfiguration();

    }

}