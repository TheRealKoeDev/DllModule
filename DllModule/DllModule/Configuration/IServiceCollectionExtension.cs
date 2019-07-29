using KoeLib.DllModule.Settings;
using KoeLib.DllModule.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDllModules(this IServiceCollection serviceCollection, IConfiguration configuration, string appSettingsPath = null)
        {
            Args.ThrowExceptionIfNull(serviceCollection, nameof(serviceCollection), configuration, nameof(configuration));

            ModuleRootDirectorySettings[] settings = configuration.GetSection(appSettingsPath ?? "DllModuleSettings")?.Get<ModuleRootDirectorySettings[]>();
            if (settings == null)
            {
                throw new ConfigurationErrorsException("ModuleSettings not found.");
            }

            string appPath = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            foreach (ModuleRootDirectorySettings directorySettings in settings)
            {
                string directoryPath = appPath + Path.DirectorySeparatorChar + directorySettings.Directory;
                if (!Directory.Exists(directoryPath))
                {
                    throw new Exception();
                }

                foreach (ModuleAssemblySettings assemblySettings in directorySettings.Assemblies)
                {
                    string filePath = directoryPath + Path.DirectorySeparatorChar + assemblySettings.Filename;
                    if (!File.Exists(filePath))
                    {
                        throw new Exception();
                    }

                    Assembly moduleAssembly = Assembly.LoadFile(filePath);

                    foreach (ModuleSettings moduleSettings in assemblySettings.Modules)
                    {
                        string instaneTypeAssemblyQualifiedName = Assembly.CreateQualifiedName(moduleSettings.NamespaceOfInstance, moduleSettings.TypeOfInstance);

                        Type moduleType = moduleAssembly.GetType(moduleSettings.Type);
                        Type instanceType = Type.GetType(instaneTypeAssemblyQualifiedName);

                        if (!AreValidModuleTypes(moduleType, instanceType))
                        {
                            throw new Exception("Invalid moduleTypes.");
                        }

                        object moduleObject = Activator.CreateInstance(moduleType);
                        object instanceTypeobject = Activator.CreateInstance(instanceType);

                        Type moduleContainerType = typeof(Module<>).MakeGenericType(instanceType);

                        var moduleContainerObject = moduleContainerType.GetConstructor(new Type[] { instanceType }).Invoke(new object[] { instanceTypeobject });
                    
                        serviceCollection.AddSingleton(moduleContainerType.GetInterface($"IModule`1"), moduleContainerObject);               
                        moduleType.GetMethod("Initialize").Invoke(moduleObject, new object[1] { instanceTypeobject });
                    }
                }
            }

            return serviceCollection;
        }

        private static bool AreValidModuleTypes(Type moduleType, Type instanceType)
        {
            if (moduleType == null || instanceType == null)
                return false;            

            Type interfaceType = moduleType.GetInterface("IInitializable`1");
            if (interfaceType == null)
                return false;

            bool isIInitializable = interfaceType.GenericTypeArguments[0].IsAssignableFrom(instanceType);
            bool isCorrectNamespace = interfaceType.Namespace == $"{nameof(KoeLib)}.{nameof(DllModule)}";

            if (!isIInitializable || !isCorrectNamespace)
                return false;

            if (!moduleType.IsClass || moduleType.GetConstructor(Type.EmptyTypes) == null)
                return false;
            else if (!instanceType.IsClass || instanceType.GetConstructor(Type.EmptyTypes) == null)
                return false;

            return true;
        }        
    }
}
