using System;
using Ninject;
using Ninject.Activation;

namespace App.Library.Ninject.Configuration
{
    /// <summary>
    ///     Configures an Ninject kernel
    /// </summary>
    public interface IDependencyConfiguration
    {

        /// <summary>
        ///     Applies the configuration to the specified kernel
        /// </summary>
        /// <param name="kernel">
        ///     The kernel to be configured
        /// </param>
        /// <param name="scope">
        ///     The scope that should be applied when to be configured
        /// </param>

        void configure(IKernel kernel, Func<IContext, object> scope);

    }
}
