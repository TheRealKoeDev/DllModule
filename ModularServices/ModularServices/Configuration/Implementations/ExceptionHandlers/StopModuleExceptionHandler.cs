using KoeLib.ModularServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularServices.Configuration.Implementations.ExceptionHandlers
{
    internal class StopModuleExceptionHandler<TService> : IModuleExceptionHandler<TService>
        where TService : class
    {
        public OnModuleExceptionAction Handle(Exception e, TService service, IModule<TService> module, ModuleExceptionLocation location)
            => OnModuleExceptionAction.Stop;
    }
}
