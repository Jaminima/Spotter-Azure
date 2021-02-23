using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class Worker : BackgroundService
    {
        #region Fields

        private readonly ILogger<Worker> _logger;

        #endregion Fields

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Actions.Watcher.OnSkip = Actions.AutoSkipRemover.Skipped;
            Actions.Watcher.OnNextSong = Actions.BetterShuffle.OnNextSong;

            while (!stoppingToken.IsCancellationRequested)
            {
                Actions.Watcher.CheckEvents();
                _logger.LogInformation("Worker ran at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        #endregion Methods

        #region Constructors

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        #endregion Constructors
    }
}
