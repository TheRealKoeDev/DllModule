# Modular-Services v2.0.0
This Library allows you to bind classes, that implement a provided interface, from Dlls to Services in .NetCore by notating the dll/class in the configuration of your Project, without to recompile or restart your application.

Inspired by the concept of more extensible Applications by Miguel Castro  
https://www.youtube.com/watch?v=jy-ZV7uEm7g  
https://www.pluralsight.com/courses/developing-extensible-software  

#### Namespace: KoeLib.ModularServices
* Configuration: KoeLib.ModularServices.Configuration
* AppSettings: KoeLib.ModularServices.Settings

#### NuGet
````
Install-Package Modular-Services
````

## Usage Example

#### Startup.cs
````csharp
//TestService and TestHandler can use Dependencyinjection

public IConfiguration Configuration { get; }

public Startup(IConfiguration configuration)
{
    Configuration = configuration;            
}
        
public void ConfigureServices(IServiceCollection services)
{
    services.AddModularServices(Configuration, "KoeLib:ModularServices", serviceGenerator =>
    {
        serviceGenerator.AddScoped<TestService>(conf => {
            //The defaultbehavior, if no handler is set, is allways to continue.
            conf.SetExceptionHandler<TestHandler>();
        });
    });
}      
````

#### appsettings.json
````javascript
{
  "KoeLib": {
    "ModularServices": {
        "TestLibrary.TestService": //Is Fullname of the Type of the Service
        [
            {
              "Ignore": false, //Optional, default value is false
              "DllPath": "<path to folder>\\TestModule.dll",
              "PathType": "Absolute",
              "FullNameOfType": "TestModule.Module",

              //Actions are optional and have priority over the set handler
              "OnConfigApplyExceptionAction": "Continue", 
              "OnConstructorExceptionAction": "Stop",
              "OnInitializeExceptionAction": "Throw"
            }
        ]
    }
  }
} 
````

#### TestModule.dll
````csharp
using TestLibrary;
using KoeLib.ModularServices;

namespace TestModule
{
  //Needs a Default Constructor to work.
  public class Module : IModule<TestService>
  {
      private readonly TestService _service;

      //Inherited from IModule<TestService>
      public void Initialize(TestService service)
      {
          _service = service;
          //Do stuff
      }
  }
}
````

#### TestController.cs
````csharp
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly TestService _service;
    
    //public TestController(TestService Service){} 
    //also works, but Modules are not called in this case.
    public TestController(IModularService<TestService> modularService)
    {
        _service = modularService.Service;
    }
    
    .
    .
    .
}
````
