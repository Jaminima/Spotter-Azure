using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Service
{
    public class Program
    {
        #region Methods

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddDbContextFactory<Model.Models.SpotterAzure_dbContext>();
                });

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}
