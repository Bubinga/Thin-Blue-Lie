using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThinBlueLie.Identity;
using DataAccessLibrary.DataAccess;
using Syncfusion.Blazor;
using ThinBlueLie.Helper;
using AutoMapper;
using DataAccessLibrary.UserModels;
using ThinBlueLie.Searches;
using DiffPlex.DiffBuilder;
using DiffPlex;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using ThinBlueLie.Models;
using ThinBlueLie.Helper.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Dapper.Logging;
using MySqlConnector;
using Dapper.Logging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace ThinBlueLie
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("UserDB"), new MySqlServerVersion(new Version(5, 7, 17))));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            });

           
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit 
                // configuration.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddDbConnectionFactory(prv => new MySqlConnection(Configuration.GetConnectionString("DataDB")),
                options => options
                    .WithLogLevel(LogLevel.Information)
                    .WithSensitiveDataLogging(),
                ServiceLifetime.Singleton);

            services.AddAuthentication()
                .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            });

            services.AddOptions();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSignalR();
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddHeadElementHelper();

            services.AddHttpContextAccessor();

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddScoped<ISideBySideDiffBuilder, SideBySideDiffBuilder>();
            services.AddScoped<IDiffer, Differ>();

            services.AddSingleton<CircuitHandler, TrackingCircuitHandler>();
            services.AddSingleton<IDataAccess, DataAccess>();
            services.AddSingleton<SearchesTimeline>();
            services.AddSingleton<SearchesSubmit>();
            services.AddSingleton<SearchesEditReview>();
            services.AddSingleton<SearchesFlagReview>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSyncfusionBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjAxMzA0QDMxMzkyZTM0MmUzMFBEWGFnQ2ZHc1hzMWxHWFZUNmczTTJ1QkFYVndOSHZsOHEzZkRwRUhHQW89");
            ConnectionString = Configuration["ConnectionStrings:DataDB"];
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {               
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHeadElementServerPrerendering();
            app.UseCertificateForwarding();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
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
