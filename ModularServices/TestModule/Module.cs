using TestLibrary;
using KoeLib.ModularServices;
using System;

namespace TestModule
{
    public class Module : IModule<ITestServiceInterface>
    {
        ITestServiceInterface _module;
        public Module()
        {

        }

        public void Initialize(ITestServiceInterface module)
        {
            _module = module;
            _module.InitializedAt = DateTime.Now;
        }
    }
}
