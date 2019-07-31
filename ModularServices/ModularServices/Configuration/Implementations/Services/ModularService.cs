
using KoeLib.ModularServices.Configuration.Dependencies;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration.Implementations.Services
{

    [DebuggerStepThrough]
    internal class ModularService<TService> : IModularService<TService>
        where TService : class
    {
        public TService Service { get; }

        internal ModularService(TService instance)
        {
            Service = instance;
        }

        public ModularService(TService instance, IServiceModuleBuilder<TService> moduleBuilder) : this(instance)
        {

            foreach (IModule<TService> module in moduleBuilder.Build())
            {
                module.Initialize(Service);
            }
        }
    }
}
