using KoeLib.ModularServices;
using KoeLib.ModularServices.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.ModularServices.Configuration.Implementations.ExceptionHandlers
{
    [DebuggerStepThrough]
    internal class ContinueExceptionHandler<TService> : IServiceExceptionHandler<TService>
        where TService : class
    {
        public OnExceptionAction HandleConfigApplyException(Exception e, ServiceModuleSettings settings, int configIndex)
            => OnExceptionAction.Continue;

        public OnExceptionAction HandleConfigLoadException(Exception e)
            => OnExceptionAction.Continue;

        public OnExceptionAction HandleModuleConstructorException(Exception e, TService service, int indexOfModule)
            => OnExceptionAction.Continue;

        public OnExceptionAction HandleModuleInitializationException(Exception e, TService service, IModule<TService> module, int indexOfModule)
            => OnExceptionAction.Continue;
    }
}
