using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Coravel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Drunkenpolls.Zapfenstreich
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string mongodbDatabase { get; set; }
        public string mongodbCollection { get; set; }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                // l8r
                scheduler.Schedule<Zapfenstreich>().Hourly();
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    services.AddScheduler();
                    services.AddTransient<Zapfenstreich>();
                });
    }
}
