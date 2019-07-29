using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    public interface IModule<TInstance>
        where TInstance: class
    {
        TInstance Instance { get; }
    }
}
