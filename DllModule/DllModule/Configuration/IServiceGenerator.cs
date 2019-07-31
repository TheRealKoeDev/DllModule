using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public interface IServiceGenerator
    {
        IServiceGenerator AddSingletonModularService<TServiceType>(Action<ISubServiceGenerator<TServiceType>> subServiceGeneratorAction = null)
            where TServiceType : class;

        IServiceGenerator AddScopedModularService<TServiceType>(Action<ISubServiceGenerator<TServiceType>> subServiceGeneratorAction = null)
            where TServiceType : class;

        IServiceGenerator AddTransientModularService<TServiceType>(Action<ISubServiceGenerator<TServiceType>> subServiceGeneratorAction = null)
            where TServiceType : class;
    }
}
