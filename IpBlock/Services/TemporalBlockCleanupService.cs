using IpBlock.Models;
using IpBlock.Repositories;

namespace IpBlock.Services
{

    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly ICountryRepository _repo;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(3);

        public TemporalBlockCleanupService(ICountryRepository repo)
        {
            _repo = repo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                    IEnumerable<string> expiredCodes = _repo.GetExpiredTemporalBlocks();
                    foreach (string code in expiredCodes)
                        _repo.RemoveBlocked(code);
                    

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

}
