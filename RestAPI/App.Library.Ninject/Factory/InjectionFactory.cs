using System;
using System.Linq;
using App.Library.CodeStructures.Creational;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;

namespace App.Library.Ninject.Factory
{
    public class InjectionFactory<T> : IFactory<T>
    {
        private readonly IKernel _kernel;
        private readonly IParameter[] _contextParameters;

        public InjectionFactory(IContext injectionContext)
        {
            _contextParameters = injectionContext.Parameters
                                                    .Where(p => p.ShouldInherit)
                                                    .ToArray();
            _kernel = injectionContext.Kernel;
        }

        public T create()
        {
            try
            {
                return _kernel.Get<T>(_contextParameters.ToArray());
            }
            catch (Exception)
            {
                throw new Exception(
                    string.Format("An error occurred while attempting to instantiate an object of type <{0}>",
                        typeof(T)));
            }
        }
    }
}
