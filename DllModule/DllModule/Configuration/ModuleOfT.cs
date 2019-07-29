using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.DllModule.Configuration
{
    internal class Module<TInstance> : IModule<TInstance>
        where TInstance : class
    {
        public TInstance Instance { get; }

        public Module(TInstance instance)
        {
            Instance = instance;
        }       
    }
}
