using DllModuleTestApiLibrary;
using KoeLib.DllModule;
using System;

namespace DllTestModule
{
    public class Test : IModule<TestSingleton>
    {
        TestSingleton _module;
        public Test()
        {

        }

        public void Initialize(TestSingleton module)
        {
            _module = module;
            _module.InitializedAt = DateTime.Now;
        }
    }
}
