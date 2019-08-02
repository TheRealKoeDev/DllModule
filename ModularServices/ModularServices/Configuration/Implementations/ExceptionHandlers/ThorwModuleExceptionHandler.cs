using KoeLib.ModularServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularService.Configuration.Implementations.ExceptionHandlers
{
    internal class ThorwModuleExceptionHandler<TService> : IModuleExceptionHandler<TService>
        where TService : class
    {
        public OnModuleExceptionAction Handle(Exception e, TService service, IModule<TService> module, ModuleExceptionLocation location)
            => OnModuleExceptionAction.Throw;
    }
}
