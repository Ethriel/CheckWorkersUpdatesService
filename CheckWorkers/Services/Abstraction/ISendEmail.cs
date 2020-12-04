using CheckWorkers.Entity;
using System.Collections.Generic;

namespace CheckWorkers.Services.Abstraction
{
    public interface ISendEmail
    {
        void SendBasicEmail(IEnumerable<Worker> workers);
        void SendTo(string email, IEnumerable<Worker> workers);
    }
}
