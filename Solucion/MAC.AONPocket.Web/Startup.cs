using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace MAC.AONPocket.Web
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
			services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc(options => options.EnableEndpointRouting = false).AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.PropertyNamingPolicy = null;
				o.JsonSerializerOptions.DictionaryKeyPolicy = null;
			});
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
			services.AddSession();
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
                app.UseExceptionHandler("/Dashboards/Dashboard_1");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
			app.UseRouting();
			app.UseCookiePolicy();

			app.UseSession();
			app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                 name: "Medicamentos",
                 areaName: "Medicamentos",
                 pattern: "Medicamentos/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=Home}/{action=Index}");
				endpoints.MapControllerRoute(
                    name: "default",
					pattern: "{controller=Home}/{action=Index}");
			});
        }
    }
}
