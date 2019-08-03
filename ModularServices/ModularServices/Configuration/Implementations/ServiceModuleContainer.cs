using KoeLib.ModularServices.Settings;
using KoeLib.ModularServices.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

namespace KoeLib.ModularServices.Configuration.Implementations
{
    [DebuggerStepThrough]
    internal class ServiceModuleContainer<TService> : IServiceModuleContainer<TService>
        where TService : class
    {
        private Queue<ServiceCallInfo<TService>> _constructors = new Queue<ServiceCallInfo<TService>>();

        private readonly IServiceExceptionHandler<TService> _exceptionHandler;
        private readonly IConfiguration _configuration;

        IReadOnlyCollection<ServiceCallInfo<TService>> IServiceModuleContainer<TService>.CallInformation => _constructors;

        public ServiceModuleContainer(IOptionsMonitor<ModularServiceSettingsList<TService>> monitor, IServiceExceptionHandler<TService> exceptionHandler, IConfiguration config)
        {
            _exceptionHandler = exceptionHandler;
            _configuration = config;

            monitor.OnChange(ApplySettings);

            try
            {
                ApplySettings(monitor.CurrentValue, null);
            }
            catch(Exception e)
            {
                switch (_exceptionHandler.HandleConfigLoadException(e))
                {
                    case OnExceptionAction.Throw: throw;
                    case OnExceptionAction.Stop: return;
                    case OnExceptionAction.Continue: break;
                }
            }
        }

        private void ApplySettings(ModularServiceSettingsList<TService> settingsList, string text)
        {
            Queue<ServiceCallInfo<TService>> newConstructors = new Queue<ServiceCallInfo<TService>>();         

            for (int i = 0; i < settingsList.Count; i++)
            {
                ServiceModuleSettings moduleSettings = settingsList[i];

                if (settingsList[i] == null)
                {
                    try { throw new NullReferenceException($"{nameof(moduleSettings)} at index {i} is null."); }
                    catch (Exception e)
                    {
                        switch (_exceptionHandler.HandleConfigApplyException(e, moduleSettings, i))
                        {
                            case OnExceptionAction.Throw: throw;
                            case OnExceptionAction.Stop: _constructors = newConstructors; return;
                            case OnExceptionAction.Continue: continue;
                        }
                    }
                }
                else if (settingsList[i].Ignore)
                {
                    continue;
                }

                try
                {
                    string assemblyPath = moduleSettings.PathType == PathType.Absolute ?
                        moduleSettings.DllPath :
                        Path.Combine(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey), moduleSettings.DllPath);

                    Type moduleType = TypeHelper.LoadModuleTypeFromAssembly(assemblyPath, moduleSettings.FullNameOfType, typeof(IModule<TService>));

                    ServiceCallInfo<TService> callInfo = new ServiceCallInfo<TService>
                    {
                        Constructor = Expression.Lambda<Func<IModule<TService>>>(Expression.New(moduleType)).Compile(),
                        OnConstructorExceptionAction = settingsList[i].OnConstructorExceptionAction,
                        OnInitializeExceptionAction = settingsList[i].OnInitializeExceptionAction
                    };

                    newConstructors.Enqueue(callInfo);
                }
                catch (Exception e)
                {
                    switch (moduleSettings.OnConfigApplyExceptionAction ?? _exceptionHandler.HandleConfigApplyException(e, moduleSettings, i))
                    {
                        case OnExceptionAction.Throw: throw;
                        case OnExceptionAction.Stop: _constructors = newConstructors; return;
                        case OnExceptionAction.Continue: continue;
                    }
                }
            }

            _constructors = newConstructors;
        }
    }
}
