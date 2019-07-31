using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularServices
{
    public interface IModularSubService<TService, TSubService> : IModularService<TSubService>
        where TService: class
        where TSubService: class
    {
    }
}
