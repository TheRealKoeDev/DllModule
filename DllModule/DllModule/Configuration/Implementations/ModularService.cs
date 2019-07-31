using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration.Implementations
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
