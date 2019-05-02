using App.Library.Ninject.Configuration;
using App.Services.Connections.Remove;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Connections.Remove
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IRemoveConnection>()
                    .To<RemoveConnection>();

        }
    }
}
