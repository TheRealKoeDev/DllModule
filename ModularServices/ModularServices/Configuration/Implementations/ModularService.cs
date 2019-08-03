using KoeLib.ModularServices.Settings;
using System;
using System.Diagnostics;
using System.Linq;

namespace KoeLib.ModularServices.Configuration.Implementations
{
    [DebuggerStepThrough]
    internal class ModularService<TService> : IModularService<TService>
        where TService : class
    {
        public TService Service { get; }
        
        internal ModularService(TService instance)
        {
            Service = instance;
        }

        public ModularService(TService instance, IServiceModuleContainer<TService> moduleBuilder, IServiceExceptionHandler<TService> exceptionHandler) : this(instance)
        {
            for (int i = 0; i < moduleBuilder.CallInformation.Count; i++)
            {
                ServiceCallInfo<TService> callInfo = moduleBuilder.CallInformation.ElementAt(i);
                IModule<TService> module = default;

                try
                {
                    
                    module = callInfo.Constructor();
                }
                catch(Exception e)
                {
                    switch (callInfo?.OnConstructorExceptionAction ?? exceptionHandler.HandleModuleConstructorException(e, instance, i))
                    {
                        case OnExceptionAction.Continue: continue;
                        case OnExceptionAction.Stop: return;
                        case OnExceptionAction.Throw: throw;
                    }
                }

                try
                {
                    module?.Initialize(instance);
                }
                catch (Exception e)
                {
                    switch (callInfo?.OnInitializeExceptionAction ?? exceptionHandler.HandleModuleInitializationException(e, instance, module, i))
                    {
                        case OnExceptionAction.Continue: continue;
                        case OnExceptionAction.Stop: return;
                        case OnExceptionAction.Throw: throw;
                    }
                }
            }
        }
    }
}
