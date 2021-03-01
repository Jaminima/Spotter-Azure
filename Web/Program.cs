using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace Spotter_Azure
{
    public class Program
    {
        #region Methods

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
//.ConfigureAppConfiguration((context, config) =>
//{
//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
//config.AddAzureKeyVault(
//keyVaultEndpoint,
//new DefaultAzureCredential());
//})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            Actions.Log.Add("App Started", Actions.LogError.Success);
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}
