using App.Library.Ninject.Configuration;
using App.Services.Connections.Update;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Connections.Update
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IUpdateConnection>()
                    .To<UpdateConnection>();

            kernel.Bind<IUpdateConnectionLastSeen>()
                    .To<UpdateConnectionLastSeen>();

            kernel.Bind<IUpdateSignalrConnection>()
                    .To<UpdateSignalrConnection>();
        }
    }
}
