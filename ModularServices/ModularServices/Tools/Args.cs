using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KoeLib.ModularServices.Tools
{
    [DebuggerStepThrough]
    internal static class Args
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowExceptionIfNull(object arg1, string nameOfArg1)
        {
            _ = arg1 ?? throw new ArgumentNullException(nameOfArg1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowExceptionIfNull(object arg1, string nameOfArg1, object arg2, string nameOfArg2)
        {
            _ = arg1 ?? throw new ArgumentNullException(nameOfArg1);
            _ = arg2 ?? throw new ArgumentNullException(nameOfArg2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowExceptionIfNull(object arg1, string nameOfArg1, object arg2, string nameOfArg2, object arg3, string nameOfArg3)
        {
            _ = arg1 ?? throw new ArgumentNullException(nameOfArg1);
            _ = arg2 ?? throw new ArgumentNullException(nameOfArg2);
            _ = arg3 ?? throw new ArgumentNullException(nameOfArg3);
        }
    }
}
