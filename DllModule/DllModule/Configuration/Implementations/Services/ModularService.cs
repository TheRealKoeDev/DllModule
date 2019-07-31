
using KoeLib.DllModule.Configuration.Dependencies;

namespace KoeLib.DllModule.Configuration.Implementations.Services
{
    internal class ModularService<TService> : IModularService<TService>
        where TService : class
    {
        public TService Service { get; }

        public ModularService(TService instance, IServiceModuleBuilder<TService> moduleBuilder)
        {
            Service = instance;

            foreach (IModule<TService> module in moduleBuilder.Build())
            {
                module.Initialize(Service);
            }
        }
    }
}
