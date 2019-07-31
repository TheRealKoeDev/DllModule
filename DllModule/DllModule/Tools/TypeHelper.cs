using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace KoeLib.DllModule.Tools
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
                throw new TypeLoadException($"Type not found, name: {typeFullName}.");
            }
            else if (!ContainsInterface(type, moduleType))
            {
                throw new TypeLoadException($"The type does not inherit from {moduleType.FullName}.");
            }
            else if (!HasDefaultConstructor(type))
            {
                throw new TypeLoadException($"The type {typeFullName} does not have a default constructor.");
            }

            return type;
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
                throw new FileNotFoundException("Dll of Modular service not found.");
            }
            else if (!dllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("File of Modular service is no Dll.");
            }

            return Assembly.LoadFrom(dllPath);
        }


    }
}
