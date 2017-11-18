using DynTech.IdentityServer.Data.Test;
using IdentityServer4;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynTech.IdentityServer
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddIdentityServerWithMongoDB(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            var connectionStr = Configuration.GetConnectionString("MongoDbConnection");

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                })
                .AddConfigurationStore(Configuration.GetSection("MongoDB"))
                .AddOperationalStore(Configuration.GetSection("MongoDB"))
                .AddDeveloperSigningCredential()
                .AddExtensionGrantValidator<ExtensionGrantValidator>()
                .AddExtensionGrantValidator<NoSubjectExtensionGrantValidator>()
                .AddJwtBearerClientAuthentication()
                .AddAppAuthRedirectUriValidator()
                .AddTestUsers(TestUsers.Users);

            return services;
        }
    }
}