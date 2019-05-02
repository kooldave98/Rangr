using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Users.Update;
using App.Services.Users.GetById;

namespace App.Domain.Users.GetById
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IGetUserById>()
                    .To<GetUserById>();

        }
    }
}
