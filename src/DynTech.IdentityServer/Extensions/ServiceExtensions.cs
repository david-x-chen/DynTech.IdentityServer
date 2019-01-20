using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoIdentity = Microsoft.AspNetCore.Identity.MongoDB;
using IdentityServer4.Services;
using DynTech.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using DynTech.IdentityServer.Models;
using System;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DynTech.IdentityServer.Data;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// Service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Build specific section
        /// </summary>
        /// <returns>The section and connection string</returns>
        /// <param name="Configuration">Configuration.</param>
        public static Tuple<string, IConfigurationSection> SetConnectionSection(IConfiguration Configuration)
        {
            // get environment variables
            var envVar = Environment.GetEnvironmentVariables();
            var dbHost = string.Empty;
            var dbReplica = string.Empty;
            var dbUser = string.Empty;
            var dbPwd = string.Empty;

            if (envVar["DB_HOST"] != null)
            {
                dbHost = envVar["DB_HOST"].ToString();
            }

            if (envVar["DB_REPLICA"] != null)
            {
                dbReplica = envVar["DB_REPLICA"].ToString();
            }

            if (envVar["DB_USER"] != null)
            {
                dbUser = envVar["DB_USER"].ToString();
            }

            if (envVar["DB_PWD"] != null)
            {
                dbPwd = envVar["DB_PWD"].ToString();
            }

            System.Console.WriteLine($"db host:{dbHost} {dbReplica}");

            // get original section from json file
            var mongodb = Configuration.GetSection("MongoDB");
            var connectionStr = mongodb.GetValue<string>("ConnectionString");
            if (!string.IsNullOrEmpty(dbHost))
            {
                if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPwd))
                {
                    dbHost = string.Format("{0}:{1}@{2}", dbUser, dbPwd, dbHost);
                }

                connectionStr = string.Format(connectionStr, dbHost);
            }
            if (!string.IsNullOrEmpty(dbReplica))
            {
                connectionStr += $"?{dbReplica}";
            }

            var dict = new Dictionary<string, string>{
                {"MongoDB:ConnectionString", connectionStr},
                {"MongoDB:Database", mongodb.GetValue<string>("Database")}
            };

            // build a new section for mongodb
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(dict);

            Configuration = builder.Build();

            connectionStr += "/" + mongodb.GetValue<string>("Database");

            System.Console.WriteLine($"db connection:{connectionStr}");

            return new Tuple<string, IConfigurationSection>(
                connectionStr, Configuration.GetSection("MongoDB")
            );
        }

        /// <summary>
        /// Adds the identity server with mongo db.
        /// </summary>
        /// <returns>The identity server with mongo db.</returns>
        /// <param name="services">Services.</param>
        /// <param name="Configuration">Configuration.</param>
        public static IServiceCollection AddIdentityServerWithMongoDB(this IServiceCollection services, IConfiguration Configuration)
        { 
            var configedSections = SetConnectionSection(Configuration);
            var connectionStr = configedSections.Item1;
            var mongodb = configedSections.Item2;
            
            services.AddTransient<IProfileService, UserClaimsProfileService>();

            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser, MongoIdentity.IdentityRole>(connectionStr)
                    .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                    options.Authentication.CookieLifetime = new TimeSpan(24, 0, 0);
                    options.Authentication.CookieSlidingExpiration = false;
                })
                .AddConfigurationStore(mongodb)
                .AddOperationalStore(mongodb)
                .AddDeveloperSigningCredential(false)
                .AddExtensionGrantValidator<ExtensionGrantValidator>()
                .AddExtensionGrantValidator<NoSubjectExtensionGrantValidator>()
                .AddJwtBearerClientAuthentication()
                .AddAppAuthRedirectUriValidator()
                .AddProfileService<UserClaimsProfileService>()
                .AddAspNetIdentity<ApplicationUser>();
            
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAppVersionService, AppVersionService>();

            return services;
        }

        /// <summary>
        /// Adds the external identity providers.
        /// </summary>
        /// <returns>The external identity providers.</returns>
        /// <param name="services">Services.</param>
        /// <param name="Configuration">Configuration.</param>
        public static IServiceCollection AddExternalIdentityProviders(this IServiceCollection services, IConfiguration Configuration)
        {
            // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
            services.AddOidcStateDataFormatterCache();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("View Users",
                                  policy => policy.RequireClaim(CustomClaimTypes.Permission, "users.view"));
                options.AddPolicy("View Clients",
                                  policy => policy.RequireClaim(CustomClaimTypes.Permission, "clients.view"));
                options.AddPolicy("View Resources",
                                  policy => policy.RequireClaim(CustomClaimTypes.Permission, "idres.view", "apires.view"));
            });

            var authSection = Configuration.GetSection("Authentication");

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = authSection.GetSection("Google").GetValue<string>("clientId");
                    options.ClientSecret = authSection.GetSection("Google").GetValue<string>("clientSecret");
                })
                .AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                    twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
                    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
                })
                .AddCookie("MyCookie", options =>
                {
                    options.LoginPath = "/Account/Unauthorized/";
                    options.AccessDeniedPath = "/Account/Forbidden/";
                });

            return services;
        }
    }
}