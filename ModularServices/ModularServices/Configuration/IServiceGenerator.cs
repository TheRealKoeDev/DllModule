using KoeLib.ModularServices.Configuration.Dependencies;
using System;
using System.Diagnostics;

namespace KoeLib.ModularServices.Configuration
{
    public interface IServiceGenerator
    {
        IServiceGenerator AddSingleton(Type serviceType);
        IServiceGenerator AddSingleton(Type serviceType, Type implementationType);
        IServiceGenerator AddSingleton<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class;

        IServiceGenerator AddSingleton<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService;

        IServiceGenerator AddScoped(Type serviceType);
        IServiceGenerator AddScoped(Type serviceType, Type implementationType);
        IServiceGenerator AddScoped<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class;
        IServiceGenerator AddScoped<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService;

        IServiceGenerator AddTransient(Type serviceType);
        IServiceGenerator AddTransient(Type serviceType, Type implementationType);
        IServiceGenerator AddTransient<TService>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class;
        IServiceGenerator AddTransient<TService, TServiceImplementation>(Action<ISubServiceGenerator<TService>> subServiceGeneratorAction = null)
            where TService : class
            where TServiceImplementation : class, TService;
    }
}
