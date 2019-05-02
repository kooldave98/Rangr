using App.Library.Ninject.Configuration;
using App.Services.Connections.Get;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Connections.Get
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IGetConnections>()
                    .To<GetConnections>();

        }
    }
}
