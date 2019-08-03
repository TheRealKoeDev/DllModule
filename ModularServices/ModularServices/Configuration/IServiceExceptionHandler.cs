using KoeLib.ModularServices;
using KoeLib.ModularServices.Settings;
using System;

namespace KoeLib.ModularServices.Configuration
{
    public interface IServiceExceptionHandler<TService>
        where TService: class
    {
        OnExceptionAction HandleConfigLoadException(Exception e);
        OnExceptionAction HandleConfigApplyException(Exception e, ServiceModuleSettings settings, int configIndex);
        OnExceptionAction HandleModuleConstructorException(Exception e, TService service, int indexOfModule);
        OnExceptionAction HandleModuleInitializationException(Exception e, TService service, IModule<TService> module, int indexOfModule);
    }
}
