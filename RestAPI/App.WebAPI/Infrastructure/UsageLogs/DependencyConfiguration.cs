using System;
using App.Library.Ninject.Configuration;
using Ninject;
using Ninject.Activation;

namespace App.WebAPI.Infrastructure.UsageLogs
{
    public class UsageLogsDependencyConfiguration : ADependencyConfiguration
    {
        public override void configure(IKernel kernel, Func<IContext, object> scope)
        {
            kernel.Bind<UsageLogRepository>()
                    .To<UsageLogRepository>().InSingletonScope();

        }
    }
}