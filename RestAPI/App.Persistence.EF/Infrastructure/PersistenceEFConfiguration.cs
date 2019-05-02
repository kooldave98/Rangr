using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;

namespace App.Persistence.EF.Infrastructure {

    public class PersistenceEFConfiguration : IDependencyConfiguration  {

        /// <summary>
        ///     Load all the configurations in this ass
        /// </summary>
        /// <param name="kernel">
        ///     The kernel to be configured
        /// </param>
        /// <param name="scope">
        ///     The scope to use for any scope sensitive configurations.
        /// </param>
        public void configure ( IKernel kernel, Func<IContext, object> scope ) {
            var configurator = new AssemblyConfiguration( GetType(  ).Assembly );

            configurator.configure( kernel, scope );
        }
    }
}