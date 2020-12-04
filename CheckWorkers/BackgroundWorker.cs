using CheckWorkers.Services.Abstraction;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CheckWorkers
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> logger;
        private readonly ICheckWorkersForUpdates checkWorkers;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, ICheckWorkersForUpdates checkWorkers)
        {
            this.logger = logger;
            this.checkWorkers = checkWorkers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await checkWorkers.CheckWorkers();
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
