using DllModuleTestApiLibrary;
using KoeLib.DllModule;
using System;

namespace DllTestModule
{
    public class Test : IModule<TestService>
    {
        TestService _module;
        public Test()
        {

        }

        public void Initialize(TestService module)
        {
            _module = module;
            _module.InitializedAt = DateTime.Now;
        }
    }
}
