using CheckWorkers.Entity;
using CheckWorkers.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CheckWorkers.Services.Implementation
{
    public class CheckWorkersForUpdates : ICheckWorkersForUpdates
    {
        private readonly WorkerCompanyPetContext context;
        private readonly ISendEmail sendEmail;
        private readonly ILogger<CheckWorkersForUpdates> logger;

        public CheckWorkersForUpdates(WorkerCompanyPetContext context, ISendEmail sendEmail, ILogger<CheckWorkersForUpdates> logger)
        {
            this.context = context;
            this.sendEmail = sendEmail;
            this.logger = logger;
        }
        public void CheckWorkers()
        {
            var workers = context.Worker.Include(x => x.Company);
            var now = DateTime.Now;
            var fiveMinutesBefore = now.AddMinutes(-5);
            var dateString = $"{now.ToLongDateString()}, {now.ToShortTimeString()} -";
            var updatedWorkers = workers.Where(x => x.TimeUpdated > fiveMinutesBefore && x.TimeUpdated <= now);

            if (updatedWorkers.Any())
            {
                logger.LogInformation($"{dateString} {updatedWorkers.Count()} workers were updated");
                sendEmail.SendBasicEmail(updatedWorkers);
            }
            else
            {
                logger.LogInformation($"{dateString} No workers were updated");
            }
        }
    }
}
