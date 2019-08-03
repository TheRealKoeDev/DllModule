using KoeLib.ModularServices.Configuration.Implementations;
using System;
using System.Collections.Generic;

namespace KoeLib.ModularServices.Configuration
{
    internal interface IServiceModuleContainer<TService>
        where TService: class
    {
        IReadOnlyCollection<ServiceCallInfo<TService>> CallInformation { get; }
    }
}
