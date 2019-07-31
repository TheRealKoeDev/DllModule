using KoeLib.DllModule.Configuration.Implementations;
using KoeLib.DllModule.Settings;
using KoeLib.DllModule.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static ServiceGenerator AddModularServices(this IServiceCollection services, IConfiguration configuration, string settingsPath)
        {
            ModularServiceSettings[]  settings = configuration.GetSection(settingsPath).Get<ModularServiceSettings[]>() ?? new ModularServiceSettings[0];
            return new ServiceGenerator(services, configuration, settings);
        }     
    }
}
