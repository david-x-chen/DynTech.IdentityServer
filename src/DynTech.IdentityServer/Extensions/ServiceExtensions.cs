using DynTech.IdentityServer.Data.Test;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.MongoDB;
using IdentityServer4.Services;
using DynTech.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// Service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds the identity server with mongo db.
        /// </summary>
        /// <returns>The identity server with mongo db.</returns>
        /// <param name="services">Services.</param>
        /// <param name="Configuration">Configuration.</param>
        public static IServiceCollection AddIdentityServerWithMongoDB(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            var mongodb = Configuration.GetSection("MongoDB");

            services.AddTransient<IProfileService, UserClaimsProfileService>();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                })
                .AddConfigurationStore(mongodb)
                .AddOperationalStore(mongodb)
                .AddDeveloperSigningCredential()
                .AddDeveloperSigningCredential()
                .AddExtensionGrantValidator<ExtensionGrantValidator>()
                .AddExtensionGrantValidator<NoSubjectExtensionGrantValidator>()
                .AddJwtBearerClientAuthentication()
                .AddAppAuthRedirectUriValidator()
                .AddProfileService<UserClaimsProfileService>();

            var connectionStr = mongodb.GetValue<string>("ConnectionString") + "/" + mongodb.GetValue<string>("Database");
            services.AddIdentityWithMongoStores(connectionStr)
                    .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Adds the external identity providers.
        /// </summary>
        /// <returns>The external identity providers.</returns>
        /// <param name="services">Services.</param>
        /// <param name="Configuration">Configuration.</param>
        public static IServiceCollection AddExternalIdentityProviders(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
            // services.AddOidcStateDataFormatterCache("aad", "demoidsrv");

            var authSection = Configuration.GetSection("Authentication");


            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = authSection.GetSection("Google").GetValue<string>("clientId");
                    options.ClientSecret = authSection.GetSection("Google").GetValue<string>("clientSecret");
                });
            /*
                .AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";
                    options.ResponseType = "id_token";
                    options.SaveTokens = true;
                    options.CallbackPath = new PathString("/signin-idsrv");
                    options.SignedOutCallbackPath = new PathString("/signout-callback-idsrv");
                    options.RemoteSignOutPath = new PathString("/signout-idsrv");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                })
                .AddOpenIdConnect("aad", "Azure AD", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                
                    options.Authority = "https://login.windows.net/4ca9cb4c-5e5f-4be9-b700-c532992a3705";
                    options.ClientId = "96e3c53e-01cb-4244-b658-a42164cb67a9";
                    options.ResponseType = "id_token";
                    options.CallbackPath = new PathString("/signin-aad");
                    options.SignedOutCallbackPath = new PathString("/signout-callback-aad");
                    options.RemoteSignOutPath = new PathString("/signout-aad");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                })
                .AddOpenIdConnect("adfs", "ADFS", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "https://adfs.leastprivilege.vm/adfs";
                    options.ClientId = "c0ea8d99-f1e7-43b0-a100-7dee3f2e5c3c";
                    options.ResponseType = "id_token";

                    options.CallbackPath = new PathString("/signin-adfs");
                    options.SignedOutCallbackPath = new PathString("/signout-callback-adfs");
                    options.RemoteSignOutPath = new PathString("/signout-adfs");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                }); */

            return services;
        }
    }
}