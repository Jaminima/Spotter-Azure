using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class Worker : BackgroundService
    {
        #region Fields

        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<Model.Models.SpotterAzure_dbContext> dbContexts;

        #endregion Fields

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Actions.Watcher.OnSkip = Actions.AutoSkipRemover.Skipped;
            Actions.Watcher.OnNextSong = Actions.BetterShuffle.OnNextSong;

            while (!stoppingToken.IsCancellationRequested)
            {
                Actions.Watcher.CheckEvents(dbContexts.CreateDbContext());
                _logger.LogInformation("Worker ran at: {time}", DateTimeOffset.Now);
                await Task.Delay(500, stoppingToken);
            }
        }

        #endregion Methods

        #region Constructors

        public Worker(ILogger<Worker> logger, IDbContextFactory<Model.Models.SpotterAzure_dbContext> dbContexts)
        {
            this.dbContexts = dbContexts;
            _logger = logger;
        }

        #endregion Constructors
    }
}
