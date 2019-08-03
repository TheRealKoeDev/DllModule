
using System.Collections.Generic;

namespace KoeLib.ModularServices
{
    public interface IModularService<TService>
        where TService: class
    {
        TService Service { get; }

    }
}
