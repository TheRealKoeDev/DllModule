using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public interface IDllModuleConfigurator
    {
        IDllModuleConfigurator AddModule<TModule, TInstance>()
           where TModule : class, IInitializable<TInstance>
           where TInstance : class, new();

        IDllModuleConfigurator AddModule<TModule, TInstance>(TInstance instance)
           where TModule : class, IInitializable<TInstance>
           where TInstance : class;
    }
}
