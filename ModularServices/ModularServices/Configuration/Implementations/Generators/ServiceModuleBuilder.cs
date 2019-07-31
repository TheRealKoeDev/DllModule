using KoeLib.ModularServices.Configuration.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.ModularServices.Configuration.Implementations.Generators
{

    [DebuggerStepThrough]
    internal class ServiceModuleBuilder<TService> : IServiceModuleBuilder<TService>
        where TService : class
    {
        private readonly Type[] _types;

        public ServiceModuleBuilder(Type[] types) => _types = types;

        public IEnumerable<IModule<TService>> Build()
        {
            foreach (Type type in _types)
            {
                IModule<TService> module = Activator.CreateInstance(type) as IModule<TService>;
                if (module == null)
                {
                    throw new TypeInitializationException(typeof(IModule<TService>).FullName, null);
                }

                yield return module;
            }            
        }
    }
}
