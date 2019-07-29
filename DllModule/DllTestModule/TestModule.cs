using DllModuleTestApiLibrary;
using System;

namespace DllTestModule
{
    public class Test : ExternTestModule
    {
        TestSingleton _module;
        public Test()
        {

        }

        public void Initialize(TestSingleton module)
        {
            _module = module;
            module.Value = 100;
        }
    }
}
