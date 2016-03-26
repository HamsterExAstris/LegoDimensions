using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Server.Kestrel.Https;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShatteredTemple.LegoDimensions.Tracker.Models;
using ShatteredTemple.LegoDimensions.Tracker.Services;

namespace ShatteredTemple.LegoDimensions.Tracker
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.Configure<MvcOptions>(config => config.Filters.Add(new RequireHttpsAttribute()));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<SendGridOptions>(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Configure HTTPS if possible.
                var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadOnly);
                var cert = certStore.Certificates.OfType<X509Certificate2>()
                    .Where(c => c.NotBefore <= DateTime.UtcNow
                        && c.NotAfter >= DateTime.UtcNow
                        && c.SubjectName.Name == "CN=localhost")
                    .FirstOrDefault();
                if (cert != null)
                {
                    // Add workaround for URL scheme needed in ASP.NET Core RC1.
                    // This is fixed in RC2.
                    app.Use(Startup.ChangeContextToHttps);

                    // Wire up our certificate.
                    app.UseKestrelHttps(cert);
                }
                else
                {
                    loggerFactory.CreateLogger<Startup>().LogWarning("Could not find X509 certificate for localhost. Skipping HTTPS configuration.");
                }
                certStore.Close();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            this.Seed(app);

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseFacebookAuthentication(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
            app.UseGoogleAuthentication(options =>
            {
                options.AuthorizationEndpoint = this.Configuration["Authentication:Google:AuthorizationEndpoint"];
                options.CallbackPath = new PathString("/Account/ExternalLoginCallback");
                options.ClientId = this.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = this.Configuration["Authentication:Google:ClientSecret"];
                options.TokenEndpoint = this.Configuration["Authentication:Google:TokenEndpoint"];
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static RequestDelegate ChangeContextToHttps(RequestDelegate next)
        {
            return async context =>
            {
                context.Request.Scheme = "https";
                await next(context);
            };
        }

        /// <summary>
        /// Seed any necessary metadata in the database.
        /// </summary>
        private void Seed(IApplicationBuilder app)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    var seeder = new SeedData(context);
                    seeder.SeedContext();
                }
            }
            catch { }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
