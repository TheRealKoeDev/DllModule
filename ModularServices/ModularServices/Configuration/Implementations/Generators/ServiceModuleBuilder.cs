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
                if (!(Activator.CreateInstance(type) is IModule<TService> module))
                {
                    throw new TypeInitializationException(type.FullName, null);
                }

                yield return module;
            }            
        }
    }
}
