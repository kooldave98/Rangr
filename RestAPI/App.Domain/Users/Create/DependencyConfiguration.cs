using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Users.Update;
using App.Services.Users.GetById;
using App.Services.Users.Create;

namespace App.Domain.Users.Create
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<ICreateUser>()
                    .To<CreateUser>();

        }
    }
}
