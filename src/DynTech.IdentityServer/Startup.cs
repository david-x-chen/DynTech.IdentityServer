using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DynTech.IdentityServer.Services;
using DynTech.IdentityServer.Data.Seeding;
using IdentityServer4.MongoDB.Interfaces;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Start up
        /// </summary>
        /// <param name="configuration">configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServerWithMongoDB(Configuration);

            services.AddExternalIdentityProviders(Configuration);

            services.AddMvc().AddMetrics()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);;

            services.AddTransient<IEmailSender, AuthMessageSender>();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("api");
            app.UseIdentityServer();

            // Setup Databases
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SeedMongoDBData.Seeding(serviceScope);
            }

            app.UseIdentityServerMongoDBTokenCleanup(applicationLifetime);

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
