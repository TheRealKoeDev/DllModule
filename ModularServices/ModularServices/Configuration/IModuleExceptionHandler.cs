using KoeLib.ModularServices;
using System;

namespace KoeLib.ModularServices.Configuration
{
    public interface IModuleExceptionHandler<TService>
        where TService: class
    {
        OnModuleExceptionAction Handle(Exception e, TService service, IModule<TService> module, ModuleExceptionLocation location);
    }
}
