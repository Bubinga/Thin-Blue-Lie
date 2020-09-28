using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThinBlueLieB.Identity;
using ThinBlueLieB.Data;
using DataAccessLibrary.DataAccess;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Syncfusion.Blazor;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Searches;
using AutoMapper;

namespace ThinBlueLieB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static string ConnectionString { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("UserDB"), MySqlOptions => MySqlOptions
                .ServerVersion(new Version(8, 0, 18), ServerType.MySql)));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            options.Password.RequireNonAlphanumeric = false
            )
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddOptions();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            //services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IDataAccess, DataAccess>();

            services.AddSyncfusionBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA2NDMyQDMxMzgyZTMyMmUzMFVGc2cvTmU0d0NMbWZiRjBkZEl2WGlzU3lHdnBIcTNYRHYzYk5OSHRFTDA9");
            ConnectionString = Configuration["ConnectionStrings:DataDB"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
