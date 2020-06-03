using AspNetCore.Introduction.Extensions;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Middlewares;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Introduction
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
            services.AddDbContext<AspNetCoreIntroductionContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("AspNetCoreIntroductionContext")));

            services.AddRepositoriesServices();
            services.AddScoped<Base, Derived>();
            services.AddLogging(builder =>
                builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConfiguration(Configuration.GetSection("Logging")));

            services.AddConfig(mandatoryInfoConfiguration =>
                Configuration.GetSection("MandatoryInfoConfiguration").Bind(mandatoryInfoConfiguration));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseImageCache();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "ImageRoute",
                    pattern: "{images}/{id}",
                    defaults: new { controller = "Categories", action = "ShowInList" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
