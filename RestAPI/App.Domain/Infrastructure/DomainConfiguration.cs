
using App.Library.Ninject.Configuration;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Infrastructure
{
    public class DomainConfiguration : IDependencyConfiguration
    {

        /// <summary>
        ///     Load all the configurations in this ass
        /// </summary>
        /// <param name="kernel">
        ///     The kernel to be configured
        /// </param>
        /// <param name="scope">
        ///     The scope to use for any scope sensitive configurations.
        /// </param>
        public void configure(IKernel kernel, Func<IContext, object> scope)
        {
            var configurator = new AssemblyConfiguration(GetType().Assembly);

            configurator.configure(kernel, scope);
        }

    }
}
