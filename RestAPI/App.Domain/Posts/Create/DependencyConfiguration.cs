using System;
using Ninject;
using Ninject.Activation;
using App.Library.Ninject.Configuration;
using App.Services.Posts.Create;

namespace App.Domain.Posts.Create
{
    public class DependencyConfiguration : ADependencyConfiguration
    {

        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {

            kernel.Bind<ICreatePost>()
                    .To<CreatePost>();

        }
    }
}
