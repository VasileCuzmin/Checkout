﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NBB.Correlation.Serilog;
using NBB.Correlation.Serilog.SqlServer;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;

namespace Checkout.Worker
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = Configuration.GetConnectionString("Checkout_Database");

            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Add(StandardColumn.LogEvent);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.With<CorrelationLogEventEnricher>()
                .WriteTo.Console()
                .WriteTo.MsSqlServerWithCorrelation(connectionString, "__Logs", autoCreateSqlTable: true,
                    columnOptions: columnOptions)
                .CreateLogger();

            var exitCode = 0;
            try
            {
                Log.Information("Starting Checkout.Worker");
                Log.Information("Messaging.TopicPrefix=" + Configuration.GetSection("Messaging")["TopicPrefix"]);
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                exitCode = 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            Environment.Exit(exitCode);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog()
                .Build();
    }
}
