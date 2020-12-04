using CheckWorkers.Entity;
using CheckWorkers.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheckWorkers.Services.Implementation
{
    public class CheckWorkersForUpdates : ICheckWorkersForUpdates
    {
        private readonly WorkerCompanyPetContext context;
        private readonly ISendEmail sendEmail;

        public CheckWorkersForUpdates(WorkerCompanyPetContext context, ISendEmail sendEmail)
        {
            this.context = context;
            this.sendEmail = sendEmail;
        }
        public async Task CheckWorkers()
        {
            var workers = await context.Worker.Include(x => x.Company).ToArrayAsync();
            var now = DateTime.Now;
            var fiveMinutesBefore = now.AddMinutes(-5);

            var updatedWorkers = workers.Where(x => x.TimeUpdated > fiveMinutesBefore && x.TimeUpdated <= now);

            if (updatedWorkers.Any())
            {
                sendEmail.SendBasicEmail(updatedWorkers);
            }
        }
    }
}
