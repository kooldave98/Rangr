using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Users.Update;

namespace App.Domain.Users.Edit
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IUpdateUser>()
                    .To<UpdateUser>();

        }
    }
}
