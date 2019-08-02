using KoeLib.ModularServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularService.Configuration
{
    public interface IModuleExceptionHandler<TService>
        where TService: class
    {
        OnModuleExceptionAction Handle(Exception e, TService service, IModule<TService> module, ModuleExceptionLocation location);
    }
}
