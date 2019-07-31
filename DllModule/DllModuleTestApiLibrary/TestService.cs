using Microsoft.Extensions.Configuration;
using System;

namespace DllModuleTestApiLibrary
{
    public class TestService: ITestServiceInterface
    {
        public TestService(IConfiguration services)
        {

        }

        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime InitializedAt { get; set; }

        public TestSubService SubService { get; } = new TestSubService();
    }

    public class TestSubService
    {

    }

    public interface ITestServiceInterface
    {
        DateTime CreatedAt { get; }
        DateTime InitializedAt { get; set; }
        TestSubService SubService { get; }
    }
}
