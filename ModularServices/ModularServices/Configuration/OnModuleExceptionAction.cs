using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.ModularService.Configuration
{
    /// <summary>
    /// Specifies what to do if a Module throws a Exception.
    /// </summary>
    public enum OnModuleExceptionAction
    {
        /// <summary>
        /// Stops to initialize other Modules.
        /// </summary>
        Stop,

        /// <summary>
        /// Continues to initialize other modules.
        /// </summary>
        Continue,

        /// <summary>
        /// Throws the Exception.
        /// </summary>
        Throw
    }
}
