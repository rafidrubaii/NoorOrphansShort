using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NoorTrust.DataAccess;
using NoorTrust.DonationFund.Api.DataAccess;
using NoorTrust.DonationFund.Api.DataAccess.SqlServer;
using NoorTrust.DonationFund.Api.Features;
using NoorTrust.DonationFund.Api.Interfaces;
using NoorTrust.DonationFund.Api.Models;
using NoorTrust.DonationFund.Api.Services;
using NoorTrust.DonationFund.Common;
using NoorTrust.DonationFund.WebUi;
using NoorTrust.DonationFund.WebUi.Data;
using NoorTrust.DonationFund.WebUI.Controllers;

namespace NoorTrust.DonationFund.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            

            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("default")
                                ));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 2;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user's stat.
                options.ValidationInterval = TimeSpan.FromHours(12);
            });


            RegisterTypes(services);



            // Add framework services.
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)

                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDistributedMemoryCache();
            services.AddSession();


            services.AddSingleton<ConfigurationService>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("Cookies", options=> {

                options.LoginPath = "/Account/login";
                options.AccessDeniedPath = "/Account/denied";
                options.Cookie.Expiration= TimeSpan.FromMinutes(120);

                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.Lax;


            });


            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

             services.AddElmah();
            // Add Kendo UI services to the services container
            services.AddKendo();


        }
        public class ClaimsTransformer : IClaimsTransformation
        {
            public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("now", DateTime.Now.ToString()));
                return Task.FromResult(principal);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseElmah();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

         

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            CheckDatabaseHasBeenDeployed(app);
        }


        private void CheckDatabaseHasBeenDeployed(IApplicationBuilder app)
        {
            using (var scope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<NoorTrustDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);

                    }
                }

                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);


                    }
                }
            }
        }
        void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();

            services.AddTransient<IFeatureManager, FeatureManager>();

            services.AddTransient<NoorTrust.DonationFund.Api.Services.ILogger, Logger>();
           
            services.AddDbContext<NoorTrustDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddTransient<INoorTrustDbContext, NoorTrustDbContext>();


            services.AddTransient<IRepository<Sponsor>, SqlEntityFrameworkSponsorRepository>();
            services.AddTransient<IRepository<SponsorActivity>, SqlEntityFrameworkSponsorActivityRepository>();
            services.AddTransient<IRepository<Orphan>, SqlEntityFrameworkOrphanRepository>();
            services.AddTransient<IRepository<OrphanFile>, SqlEntityFrameworkOrphanFileRepository>();
            services.AddTransient<IRepository<OrphanActivity>, SqlEntityFrameworkOrphanActivityRepository>();

            services.AddTransient<IRepository<Donation>, SqlEntityFrameworkDonationRepository>();
            services.AddTransient<IRepository<Report>, SqlEntityFrameworkReportRepository>();
            services.AddTransient<IRepository<ReportFile>, SqlEntityFrameworkReportFileRepository>();

            services.AddTransient<IRepository<Country>, SqlEntityFrameworkCountryRepository>();

            services.AddTransient<IValidatorStrategy<President>, DefaultValidatorStrategy<President>>();
            services.AddTransient<IDaysInOfficeStrategy, DefaultDaysInOfficeStrategy>();

            services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();



            services.AddTransient<ISponsorService, SponsorService>();
            services.AddTransient<IOrphanService, OrphanService>();
            services.AddTransient<IDonationService, DonationService>();
            services.AddTransient<IReportService, ReportService>();

            services.AddTransient<ICountryService, CountryService>();

            services.AddTransient<ISubscriptionService, SubscriptionService>();

            services.AddTransient<ITestDataUtility, TestDataUtility>();

        }


    }



    public class ConfigurationService
    {
        public IConfiguration Configuration { get; private set; }

        public IHostingEnvironment Environment { get; private set; }
        public ConfigurationService(IHostingEnvironment environment)
        {
            this.Environment = environment;

         

            //otherwise instantiate the default builder which will use the default appSettings.json and appSettings.{environment}.json files.
            var defaultBuilder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                                .Build();
            var config = defaultBuilder.Services.GetService<IConfiguration>();

            this.Configuration = config;
        }
    }



}
