using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Posts.Get;

namespace App.Domain.Posts.Get
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<IGetPosts>()
                    .To<GetPosts>();

        }
    }
}
