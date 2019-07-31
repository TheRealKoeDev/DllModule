using DllModuleTestApiLibrary;
using KoeLib.DllModule;
using System;

namespace DllTestModule
{
    public class Test : IModule<ITestServiceInterface>
    {
        ITestServiceInterface _module;
        public Test()
        {

        }

        public void Initialize(ITestServiceInterface module)
        {
            _module = module;
            _module.InitializedAt = DateTime.Now;
        }
    }
}
