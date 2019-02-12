using System;
using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Filtering;
using App.Metrics.Formatters.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// Program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();


            try
            {
                Log.Information("Getting the motors running...");

                var filter = new MetricsFilter().WhereType(MetricType.Timer);

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureMetricsWithDefaults(builder =>
                    {
                        builder.Filter.With(filter);
                        builder.Report.ToInfluxDb(
                        options =>
                        {
                            options.InfluxDb.BaseUri = new Uri(Environment.GetEnvironmentVariable("INFLUXDB_BASEURI") ?? "http://127.0.0.1:8086");
                            options.InfluxDb.Database = Environment.GetEnvironmentVariable("INFLUXDB_DATABASE") ?? "";
                            options.InfluxDb.Consistenency = Environment.GetEnvironmentVariable("INFLUXDB_CONSISTENCY") ?? "";
                            options.InfluxDb.UserName = Environment.GetEnvironmentVariable("INFLUXDB_USERNAME") ?? "";
                            options.InfluxDb.Password = Environment.GetEnvironmentVariable("INFLUXDB_PASSWORD") ?? "";
                            options.InfluxDb.RetentionPolicy = Environment.GetEnvironmentVariable("INFLUXDB_RETENTIONPOLICY") ?? "";
                            options.InfluxDb.CreateDataBaseIfNotExists = true;
                            options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                            options.HttpPolicy.FailuresBeforeBackoff = 5;
                            options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                            options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                            options.Filter = filter;
                            options.FlushInterval = TimeSpan.FromSeconds(20);
                        });
                    })
                    .UseMetrics()
                    .UseMetricsWebTracking()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseConfiguration(configuration)
                    .UseSerilog()
                    .Build();

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
