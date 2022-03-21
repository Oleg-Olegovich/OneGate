using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OneGate.Backend.Core.Shared.Logging;

namespace OneGate.Backend.Core.Engines.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseLogging();
    }
}