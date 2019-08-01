# Modular-Services
This Library allows you to bind classes, that implement a provided interface, from Dlls to Services in .NetCore by notating the dll/class in the configuration of your Project, without to recompile your application .

## Usage Example

### Startup.cs
````csharp
//TestLibrary.TestService can use Dependencyinjection

public IConfiguration Configuration { get; }

public Startup(IConfiguration configuration)
{
    Configuration = configuration;            
}
        
public void ConfigureServices(IServiceCollection services)
{
    services.AddModularServices(Configuration, "KoeLib:ModularServices", serviceGenerator =>
    {
        serviceGenerator.AddScoped<TestService>();
    });
}        
````

### appsettings.json
````javascript
{
  "KoeLib": {
    "ModularServices": [
      {
        "Typename": "TestService",
        "Namespace": "TestLibrary",
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

### TestModule.dll
````csharp
//Location: <AppPath>/TestModules

using TestLibrary;
using KoeLib.ModularServices;

namespace TestModule
{
  //Needs a Default Constructor to work.
  public class Module : IModule<TestService>
  {
      TestService _service;

      //Inherited from IModule<TestService>
      public void Initialize(TestService service)
      {
          _service = service;
          //Do stuff
      }
  }
}
````

### TestController.cs
````csharp
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly TestService Service;
    
    //public TestController(TestService Service){} 
    //also works, but Modules are not called in this case.
    public TestController(IModularService<TestService> modularService)
    {
        Service = modularService.Service;
    }
    
    .
    .
    .
}
````
