using CheckWorkers.Entity;
using CheckWorkers.Services.Abstraction;
using CheckWorkers.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;

namespace CheckWorkers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<BackgroundWorker>()
                    .Configure<EventLogSettings>(config =>
                    {
                        config.LogName = "Check workers Service";
                        config.SourceName = "Check workers for updates Source";
                    });
                    services.AddSingleton<WorkerCompanyPetContext>();
                    services.AddSingleton<ISendEmail, SendEmail>();
                    services.AddSingleton<ICheckWorkersForUpdates, CheckWorkersForUpdates>();
                }).UseWindowsService();
    }
}
