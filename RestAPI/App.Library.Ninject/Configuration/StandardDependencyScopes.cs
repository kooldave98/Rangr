using System;
using Ninject.Activation;

namespace App.Library.Ninject.Configuration
{
    public static class StandardDependencyScopes
    {
        /// <summary>
        /// A new instance is used for every request.
        /// </summary>
        public static readonly Func<IContext, object> InRequestScope = (c) => System.Web.HttpContext.Current;

        /// <summary>
        /// A single instance is used for all requests for the lifetime of the kernel
        /// </summary>
        public static readonly Func<IContext, object> InSingleScope = (c) => c.Kernel;

        /// <summary>
        /// A new instance will be used for all request within the same thread.
        /// </summary>
        public static readonly Func<IContext, object> InThreadScope = (c) => System.Threading.Thread.CurrentThread;

        /// <summary>
        /// A new instance is used for every request
        /// </summary>
        public static readonly Func<IContext, object> InTransientScope = (c) => null;
    }
}
