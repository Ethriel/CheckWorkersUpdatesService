using System.Threading.Tasks;

namespace CheckWorkers.Services.Abstraction
{
    public interface ICheckWorkersForUpdates
    {
        Task CheckWorkers();
    }
}
