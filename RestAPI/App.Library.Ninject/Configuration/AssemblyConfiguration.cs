using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Ninject;
using Ninject.Activation;
using App.Library.CodeStructures.Behavioral;

namespace App.Library.Ninject.Configuration
{
    /// <summary>
    ///     Apply all the configurations in an assembly 
    /// </summary>
    public class AssemblyConfiguration
    {


        /// <summary>
        ///     Apply the all the configurations in the assembly to the kernel for for the
        /// specified scope. 
        /// </summary>
        /// <param name="kernel">
        ///     The kernel to be configured
        /// </param>        
        /// <param name="for_scope">
        ///     Scope resolution that is to be used.
        /// </param>
        public void configure(IKernel kernel, Func<IContext, object> for_scope)
        {

            configurations.Do(c => c.configure(kernel, for_scope));
        }

        public AssemblyConfiguration(Assembly from_assembly)
        {
            Guard.IsNotNull(from_assembly, "from_assembly");

            assembly = from_assembly;

        }

        // All configurations in the assembly loaded via MEF
        private IEnumerable<IDependencyConfiguration> configurations
        {

            get
            {
                var catalog = new AssemblyCatalog(assembly);
                var container = new CompositionContainer(catalog);

                return container.GetExportedValues<IDependencyConfiguration>();
            }
        }


        private readonly Assembly assembly;
    }
}
