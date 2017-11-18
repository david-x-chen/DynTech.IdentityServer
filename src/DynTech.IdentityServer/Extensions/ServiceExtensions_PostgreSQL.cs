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
        public static IServiceCollection AddIdentityServerWithPostgreSQL(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            var connectionStr = Configuration.GetConnectionString("PostSQLConnection");

            return services;
        }
    }
}