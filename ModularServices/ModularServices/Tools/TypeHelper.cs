using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KoeLib.ModularServices.Tools
{
    [DebuggerStepThrough]
    internal static class TypeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type LoadModuleTypeFromAssembly(string assemblyFullPath, string typeFullName, Type moduleType)
        {
            Type type = LoadAssembly(assemblyFullPath).GetType(typeFullName);
            if (type == null)
            {
                throw new TypeLoadException($"{typeFullName} could not be found.");
            }
            else if (!ContainsInterface(type, moduleType))
            {
                throw new TypeLoadException($"{type.FullName} is no instance of {moduleType.FullName}.");
            }
            else if (!HasDefaultConstructor(type))
            {
                throw new TypeLoadException($"{typeFullName} does not have a default constructor.");
            }

            return type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateService(Type service)
        {
            if (!service.IsClass)
            {
                throw new ArgumentException($"{service.FullName} is not a class.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateService(Type service, Type serviceImplementation)
        {
            if (!service.IsClass && !service.IsInterface)
            {
                throw new ArgumentException($"{service.FullName} is not a class or interface.");
            }
            else if (!serviceImplementation.IsClass)
            {
                throw new ArgumentException($"{serviceImplementation.FullName} is not a class.");
            }
            else if (!service.IsAssignableFrom(serviceImplementation))
            {
                throw new ArgumentException($"{service.FullName} does not derive from {serviceImplementation.FullName}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsInterface(Type type, Type interfaceType)
            => type.GetInterfaces().Contains(interfaceType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasDefaultConstructor(Type type)
            => type.GetConstructor(Type.EmptyTypes) != null;        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Assembly LoadAssembly(string dllPath)
        {
            if (!File.Exists(dllPath))
            {
                throw new FileNotFoundException($"Assembly at {dllPath} does not exist.", dllPath);
            }
            else if (!dllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new FileLoadException($"File at {dllPath} is no dll file.", dllPath);
            }
            return Assembly.LoadFrom(dllPath);
        }
    }
}
