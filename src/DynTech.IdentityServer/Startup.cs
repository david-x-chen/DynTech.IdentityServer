using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using DynTech.IdentityServer.Models;
using DynTech.IdentityServer.Services;
using System.Linq;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using DynTech.IdentityServer.Configuration;
using Microsoft.AspNetCore.Mvc;
using DynTech.IdentityServer.Data.Seeding;
using IdentityServer4.MongoDB.Interfaces;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        IHostingEnvironment _hostingEnv;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            _hostingEnv = env;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            var defaultDb = Configuration.GetValue<string>("DefaultDb").ToUpper();

            switch(defaultDb)
            {
                case "MSSQL":
                default:
                    services.AddIdentityServerWithMSSQL(Configuration);
                    break;
                case "MONGODB":
                    services.AddIdentityServerWithMongoDB(Configuration);
                    break;
                case "POSTSQL":
                    services.AddIdentityServerWithPostgreSQL(Configuration);
                    break;
            }

            services.AddExternalIdentityProviders(Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddMvc(options => {
                //options.Filters.Add(new RequireHttpsAttribute ());
            });

            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("api");
            app.UseIdentityServer();

            var defaultDb = Configuration.GetValue<string>("DefaultDb").ToUpper();

            switch(defaultDb)
            {
                case "MSSQL":
                default:
                    SeedMSSQLData.InitializeAspNetIdentityDatabase(app);
                    SeedMSSQLData.InitializeIdentityServerDatabase(app);
                    break;
                case "MONGODB":
                    // Setup Databases
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        SeedMongoDBData.EnsureSeedData(serviceScope.ServiceProvider.GetService<IConfigurationDbContext>());
                    }

                    app.UseIdentityServerMongoDBTokenCleanup(applicationLifetime);
                    break;
                case "POSTSQL":
                    break;
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
