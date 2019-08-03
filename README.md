# Modular-Services
This Library allows you to bind classes, that implement a provided interface, from Dlls to Services in .NetCore by notating the dll/class in the configuration of your Project, without to recompile your application.

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
            conf.SetExceptionHandler<TestHandler>();
        });
    });
}      
````

#### appsettings.json
````javascript
{
  "KoeLib": {
    "ModularServices": [
      {
        "Typename": "TestService",
        "Namespace": "TestLibrary",
        "OnModuleExceptionAction": "Continue",  //Optional, default value is Throw
                                                //Is ignored if a custom ExceptionHandler is set
        "Modules": [
          {
            "DllPath": "TestModules/TestModule.dll",
            "PathType": "Relative",
            "FullNameOfType": "TestModule.Module",
            
            "IsRequired" : true, //Optional, default value is true
            "Ignore": false      //Optional, default value is false
          }
        ]
      }
    ]
  }
} 
````

#### TestModule.dll
````csharp
//Location: <AppPath>/TestModules

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
