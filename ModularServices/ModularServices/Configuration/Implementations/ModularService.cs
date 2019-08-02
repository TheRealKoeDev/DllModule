
using KoeLib.ModularService.Configuration;
using KoeLib.ModularServices.Configuration.Dependencies;
using System;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration.Implementations
{

    //[DebuggerStepThrough]
    internal class ModularService<TService> : IModularService<TService>
        where TService : class
    {
        public TService Service { get; protected set; }

        internal ModularService(TService instance)
        {
            Service = instance;
        }

        public ModularService(TService instance, IServiceModuleContainer<TService> moduleBuilder, IModuleExceptionHandler<TService> exceptionHandler) : this(instance)
        {
            foreach (Func<IModule<TService>> constructor in moduleBuilder.Constructors)
            {
                IModule<TService> module = default;

                try
                {
                    module = constructor();
                }
                catch(Exception e)
                {
                    switch (exceptionHandler.Handle(e, instance, module, ModuleExceptionLocation.Constructor))
                    {
                        case OnModuleExceptionAction.Continue: continue;
                        case OnModuleExceptionAction.Stop: return;
                        case OnModuleExceptionAction.Throw: throw;
                    }
                }

                try
                {
                    module?.Initialize(instance);
                }
                catch (Exception e)
                {
                    switch (exceptionHandler.Handle(e, instance, module, ModuleExceptionLocation.Initialization))
                    {
                        case OnModuleExceptionAction.Continue: continue;
                        case OnModuleExceptionAction.Stop: return;
                        case OnModuleExceptionAction.Throw: throw;
                    }
                }
            }
        }
    }
}
