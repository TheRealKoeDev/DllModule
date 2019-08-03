using TestLibrary;
using KoeLib.ModularServices;
using System;

namespace TestModule
{
    public class Module : IModule<TestService>
    {
        private int _test = 888;

        public Module()
        {            
            _test = 999;
        }
        
        public void Initialize(TestService module)
        {
            module.Count++;
        }
    }
}
