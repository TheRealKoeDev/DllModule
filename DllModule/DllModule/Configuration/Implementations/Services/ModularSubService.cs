using KoeLib.DllModule.Configuration.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration.Implementations.Services
{
    internal class ModularSubService<TService, TSubService> : IModularService<TSubService>
        where TService : class
        where TSubService : class
    {
        public TSubService Service { get; }

        public ModularSubService(IModularService<TService> parentService, ISubServiceSelector<TService, TSubService> selector)
        {
            Service = selector.Select(parentService.Service);
        }        
    }
}
