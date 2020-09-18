using System.IO;
using Coravel;
using Drunkenpolls.Bar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Zapfenstreich.Services;

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
                    var Config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

                    services.Configure<DrunkenpollsDatabaseSettings>(Config.GetSection(nameof(DrunkenpollsDatabaseSettings)));

                    services.AddSingleton<IDrunkenpollsDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DrunkenpollsDatabaseSettings>>().Value);

                    services.AddSingleton<GameService>();

                    services.AddScheduler();
                    services.AddTransient<Zapfenstreich>();
                });
    }

}
