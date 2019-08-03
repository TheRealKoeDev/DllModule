using TestLibrary;
using KoeLib.ModularServices.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KoeLib.ModularServices;
using KoeLib.ModularServices.Settings;
using System;

namespace TestApi
{
    public class TestHandler : IServiceExceptionHandler<TestService>
    {
        public OnExceptionAction HandleConfigApplyException(Exception e, ServiceModuleSettings settings, int configIndex)
            => OnExceptionAction.Throw;

        public OnExceptionAction HandleConfigLoadException(Exception e)
            => OnExceptionAction.Throw;

        public OnExceptionAction HandleModuleConstructorException(Exception e, TestService service, int indexOfModule)
            => OnExceptionAction.Throw;

        public OnExceptionAction HandleModuleInitializationException(Exception e, TestService service, IModule<TestService> module, int indexOfModule)
            => OnExceptionAction.Throw;
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
                serviceGenerator.AddScoped<TestService, TestService>(conf => {
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
