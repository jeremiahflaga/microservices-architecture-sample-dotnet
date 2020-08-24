using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // NOTE_JBOY: get configuration for the database from appsettings.json
            // then add it to DI container as a singleton
            // ref "Configuration should use the options pattern." - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
            // ref "Options pattern in ASP.NET Core" - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1#ioptionsmonitor
            // ref "GetRequiredService()" - https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core/
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(CatalogDatabaseSettings.SETTINGS_NAME));
            services.AddSingleton<ICatalogDatabaseSettings>(sp => 
                sp.GetRequiredService<IOptionsMonitor<CatalogDatabaseSettings>>().CurrentValue);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
