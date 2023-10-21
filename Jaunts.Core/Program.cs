
namespace Jaunts.Core.Api
{
    public class Program
    {
        public static void Main(string[] arguments) =>
            CreateHostBuilder(arguments).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] arguments)
        {
            return Host.CreateDefaultBuilder(arguments)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
        }
    }
}
