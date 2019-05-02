using App.Library.Ninject.Configuration;
using App.Services.Connections.GetForUser;
using Ninject;
using Ninject.Activation;
using System;

namespace App.Domain.Connections.GetForUser
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IGetConnectionForUser>()
                    .To<GetConnectionForUser>();

        }
    }
}
