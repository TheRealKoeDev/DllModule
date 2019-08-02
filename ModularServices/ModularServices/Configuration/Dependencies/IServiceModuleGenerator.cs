using System;
using System.Collections.Generic;

namespace KoeLib.ModularServices.Configuration.Dependencies
{
    public interface IServiceModuleContainer<TService>
        where TService: class
    {
        IEnumerable<Func<IModule<TService>>> Constructors { get; }
    }
}
