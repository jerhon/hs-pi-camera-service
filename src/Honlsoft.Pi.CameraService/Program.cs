using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Unosquare.WiringPi;

namespace Honlsoft.Pi.CameraService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Unosquare.RaspberryIO.Pi.Init<BootstrapWiringPi>();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
