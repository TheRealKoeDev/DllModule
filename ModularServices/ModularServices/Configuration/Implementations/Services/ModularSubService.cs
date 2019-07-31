using KoeLib.ModularServices.Configuration.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.ModularServices.Configuration.Implementations.Services
{

    [DebuggerStepThrough]
    internal class ModularSubService<TService, TSubService> : ModularService<TSubService>
        where TService : class
        where TSubService : class
    {

        public ModularSubService(IModularService<TService> parentService, ISubServiceSelector<TService, TSubService> selector): base(selector.Select(parentService.Service))
        {
        }        
    }
}
