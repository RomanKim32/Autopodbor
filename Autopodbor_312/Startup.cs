using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace Autopodbor_312
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
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(
                opt =>
                {
                    var supportedCulteres = new List<CultureInfo>
                    {
                        new CultureInfo("ru"),
                        new CultureInfo("ky")
                    };
                    opt.DefaultRequestCulture = new RequestCulture("ru");
                    opt.SupportedCultures = supportedCulteres;
                    opt.SupportedUICultures = supportedCulteres;
                });

            services.AddControllersWithViews();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AutopodborContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(connection))
                .AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<AutopodborContext>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
			services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IContactInformationsRepository, ContactInformationsRepository>();
            services.AddScoped<ICalculatorRepository, CalculatorRepository>();

            //services.Configure<PrivateInfoConfig>(options => Configuration.GetSection("PrivateInfo").Bind(options));
            /*var s = services.Configure<PrivateInfoConfig>(Configuration.GetSection(
            						   PrivateInfoConfig.PrivateInfo));*/

			/*var pic = new PrivateInfoConfig();
			Configuration.Bind(PrivateInfoConfig.PrivateInfo, pic);
			services.AddSingleton(pic);*/

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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
