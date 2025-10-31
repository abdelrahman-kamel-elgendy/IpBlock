using IpBlock.Models;
using IpBlock.Models.DTOs.Response;
using IpBlock.Repositories;
using System.Collections.Concurrent;

namespace IpBlock.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _repo;
        public LogService(ILogRepository repo) => _repo = repo;

        public void LogAttempt(BlockLogEntry entry) => _repo.Add(entry);
        public PagedResponse<BlockLogEntry> GetBlockedAttempts(int page, int pageSize) => _repo.GetBlockedAttempts(page < 1 ? 1 : page, pageSize < 1 ? 20 : pageSize);
    }
}
