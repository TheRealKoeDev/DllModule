using KoeLib.ModularServices.Configuration.Implementations;
using KoeLib.ModularServices.Settings;
using KoeLib.ModularServices.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace KoeLib.ModularServices.Configuration
{

    //[DebuggerStepThrough]
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddModularServices(this IServiceCollection services, IConfiguration configuration, string settingsPath, Action<IServiceGenerator> generateServiceAction)
        {
            Args.ThrowExceptionIfNull(services, nameof(services), configuration, nameof(configuration), settingsPath, nameof(settingsPath));
            Args.ThrowExceptionIfNull(generateServiceAction, nameof(generateServiceAction));

            generateServiceAction(new ServiceGenerator(services, configuration, settingsPath));
            return services;           
        }     
    }
}
