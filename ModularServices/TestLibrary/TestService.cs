using Microsoft.Extensions.Configuration;
using System;

namespace TestLibrary
{
    public class TestService: ITestServiceInterface
    {
        public int Count { get; set; }  

        public TestService(IConfiguration services)
        {
        }

    }

    public class TestSubService
    {

    }

    public interface ITestServiceInterface
    {
        int Count { get; set; }
        //DateTime CreatedAt { get; }
        //DateTime InitializedAt { get; set; }
        //TestSubService SubService { get; }
    }
}
