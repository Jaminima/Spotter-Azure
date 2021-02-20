using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                Actions.Watcher.OnSkip = Actions.AutoSkipRemover.Skipped;
                Actions.Watcher.OnNextSong = Actions.BetterShuffle.OnNextSong;
                Actions.Watcher.Start();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
