using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Spotter_Azure
{
    public class Program
    {
        #region Methods

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            Actions.Watcher.OnSkip = Actions.AutoSkipRemover.Skipped;
            Actions.Watcher.OnNextSong = Actions.BetterShuffle.OnNextSong;
            Actions.Watcher.Start();
            Actions.Log.Add("App Started", Actions.LogError.Success);
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}
