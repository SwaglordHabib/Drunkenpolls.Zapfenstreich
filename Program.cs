using Coravel;
using Drunkenpolls.Bar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Drunkenpolls.Zapfenstreich
{
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
                    services.Configure<IDrunkenpollsDatabaseSettings>(hostContext.Configuration.GetSection("DrunkenpollsDatabaseSettings"));
                    services.AddScheduler();
                    services.AddTransient<Zapfenstreich>();
                });
    }
}
