using TestLibrary;
using KoeLib.ModularServices.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KoeLib.ModularService.Configuration;
using KoeLib.ModularServices;
using System;
using System.Diagnostics;

namespace TestApi
{
    public class TestHandler : IModuleExceptionHandler<ITestServiceInterface>
    {
        public OnModuleExceptionAction Handle(Exception e, ITestServiceInterface service, IModule<ITestServiceInterface> module, ModuleExceptionLocation location)
        {
            return OnModuleExceptionAction.Continue;
        }
    }

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModularServices(Configuration, "KoeLib:ModularServices", serviceGenerator =>
            {
                serviceGenerator.AddScoped<ITestServiceInterface, TestService>(conf => {
                    conf.SetExceptionHandler<TestHandler>();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {            
            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
