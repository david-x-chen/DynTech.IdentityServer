using DynTech.IdentityServer.Models;
using DynTech.IdentityServer.Services;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DynTech.IdentityServer
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddIdentityServerWithMSSQL(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            var connectionStr = Configuration.GetConnectionString("SQLConnection");
            var migrationAssmName = typeof(DynTech.IdentityServer.Startup).Assembly.GetName().Name;

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStr));

            // Identity Server 4
            services.AddIdentityServer()
                .AddSigningCredential(IdentityServerBuilderExtensionsCrypto.CreateRsaSecurityKey())
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionStr, sql => sql.MigrationsAssembly(migrationAssmName));
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionStr, sql => sql.MigrationsAssembly(migrationAssmName));
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<UserClaimsProfileService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}