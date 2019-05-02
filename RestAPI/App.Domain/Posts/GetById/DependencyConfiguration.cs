using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Posts.GetById;

namespace App.Domain.Posts.GetById
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IGetPostById>()
                    .To<GetPostById>();

        }
    }
}
