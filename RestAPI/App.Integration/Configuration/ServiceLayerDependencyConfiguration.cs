using System;
using Ninject;
using Ninject.Activation;
using App.Domain.Infrastructure;

namespace App.Integration.Configuration
{
    public class ServiceLayerDependencyConfiguration
    {

        public static void without_persistence
                            (IKernel kernel
                            , Func<IContext, object> scope)
        {

            domain_confiuration.configure(kernel, scope);
        }

        public static void full_domain
                            (IKernel kernel
                            , Func<IContext, object> scope)
        {

            domain_confiuration.configure(kernel, scope);
        }

        private static readonly DomainConfiguration domain_confiuration = new DomainConfiguration();
    }
}
