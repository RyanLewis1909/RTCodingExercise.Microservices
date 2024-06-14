﻿using MassTransit;
using System.Net.Http.Headers;
using WebMVC;
using WebMVC.Services;

namespace RTCodingExercise.WebMVC
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            var appSetting = GetAppSettings(services);
            services.AddSingleton(appSetting);
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddHttpClient("CatalogApi", client =>
            {
                client.BaseAddress = new Uri(appSetting.CatalogBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Plate}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }

        private AppSettings GetAppSettings(IServiceCollection services)
        {
            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);
            var appSetting = appSettingSection.Get<AppSettings>();

            if (!appSetting.IsValid(out var reason))
            {
                throw new Exception($"AppSettings section is not valid due to: {reason}");
            }

            return appSetting;
        }
    }
}
