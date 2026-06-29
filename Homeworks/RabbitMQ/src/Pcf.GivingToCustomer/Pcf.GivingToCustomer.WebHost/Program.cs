using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Pcf.GivingToCustomer.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureKestrel(options => { options.ListenAnyIP(5000, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2); });

            webBuilder.UseStartup<Startup>();
        });
    }
}