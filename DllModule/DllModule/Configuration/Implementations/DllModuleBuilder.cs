using KoeLib.DllModule.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace KoeLib.DllModule.Configuration.Implementations
{
    internal class DllModuleBuilder<TModule, TInstance >
           where TModule : class, IInitializable<TInstance >
           where TInstance : class
    {
        public TInstance  Connector;

        public string AppPath;
        public IServiceCollection ServiceCollection;

        public ModuleRootDirectorySettings DirectorySettings;
        public ModuleAssemblySettings AssemblySettings;
        public ModuleSettings ModuleSettings;

        public void Build()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppPath + Path.DirectorySeparatorChar + DirectorySettings.Directory);
            if (!dirInfo.Exists)
            {
                throw new DirectoryNotFoundException("ModuleDirectory not found.");
            }

            FileInfo[] files = dirInfo.GetFiles(AssemblySettings.Filename);
            if (files.Length < 1)
            {
                throw new FileNotFoundException("Module-Dll not found.");
            }

            Assembly assembly = Assembly.LoadFile(files[0].FullName);
            TModule module;
            try
            {
                Type externalType = assembly.GetType(ModuleSettings.Type);
                if (!externalType.IsClass)
                {
                    throw new TypeLoadException("The module type is not a class.");
                }
                else if (!typeof(TModule).IsAssignableFrom(externalType))
                {
                    throw new TypeLoadException("The module type does not inherit from IInitializable<TInstance>.");
                }
                
                module = Activator.CreateInstance(externalType) as TModule;                
            }
            catch
            {
                throw;
            }

            module.Initialize(Connector);
            ServiceCollection.AddSingleton<IModule<TInstance >>(new Module<TInstance>(Connector));
        }
    }
}
