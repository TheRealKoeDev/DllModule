using DllModuleTestApiLibrary;
using KoeLib.DllModule.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DllModuleTestApi
{
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
            services.AddModularServices(Configuration, "KoeLib:ModularServices", subServiceGenerator =>
            {
                subServiceGenerator.AddTransient(typeof(ITestServiceInterface), typeof(TestService));

                //subServiceGenerator.AddScoped<ITestServiceInterface, TestService>(subServiceSelector =>
                //{
                //    subServiceSelector.AddSubService(ser => ser.SubService);
                //});
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
