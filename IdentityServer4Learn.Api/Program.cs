using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace IdentityServer4Learn.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Namespace);
                var host = CreateWebHostBuilder(args).Build();
                Log.Information("Starting web host ({ApplicationContext})...", Namespace);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Namespace);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseSerilog();

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            //(context, configuration) =>
            //{
            //    configuration
            //        .MinimumLevel.Debug()
            //        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //        .MinimumLevel.Override("System", LogEventLevel.Warning)
            //        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            //        .Enrich.FromLogContext()
            //        .WriteTo.File(@"log-.txt", rollingInterval: RollingInterval.Day)
            //        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
            //}
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationContext", Namespace)
                .Enrich.FromLogContext()
                .WriteTo.File(@"log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                //.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
