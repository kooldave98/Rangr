using App.Library.Ninject.Configuration;
using App.Services.Connections.Create;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Connections.Create
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<ICreateConnection>()
                    .To<CreateConnection>();

        }
    }
}
