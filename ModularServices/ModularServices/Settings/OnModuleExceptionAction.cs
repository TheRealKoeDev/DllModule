using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularServices.Settings
{
    /// <summary>
    /// Specifies what to do if a Module throws a Exception.
    /// </summary>
    public enum OnExceptionAction
    {
        /// <summary>
        /// Stops to proceed.
        /// </summary>
        Stop,

        /// <summary>
        /// Continues to proceed.
        /// </summary>
        Continue,

        /// <summary>
        /// Throws the Exception.
        /// </summary>
        Throw
    }
}
