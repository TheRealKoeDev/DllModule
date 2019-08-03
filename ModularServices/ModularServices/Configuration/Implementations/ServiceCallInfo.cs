using KoeLib.ModularServices.Settings;
using System;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration.Implementations
{
    [DebuggerStepThrough]
    internal class ServiceCallInfo<TService>
        where TService: class
    {
        public Func<IModule<TService>> Constructor { get; internal set;  }

        public OnExceptionAction? OnConstructorExceptionAction { get; internal set; }
        public OnExceptionAction? OnInitializeExceptionAction { get; internal set; }
        
    }
}
