using System;
using System.Collections.Generic;

namespace KoeLib.DllModule.Configuration.Dependencies
{
    public interface IServiceModuleBuilder<TService>
        where TService: class
    {
        IEnumerable<IModule<TService>> Build();
    }
}
