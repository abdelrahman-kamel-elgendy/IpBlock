using IpBlock.Models;
using IpBlock.Models.DTOs.Response;
using System.Collections.Concurrent;

namespace IpBlock.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly List<BlockLogEntry> _logs = new List<BlockLogEntry>();

        public void Add(BlockLogEntry entry) => _logs.Add(entry);
        public PagedResponse<BlockLogEntry> GetBlockedAttempts(int page, int pageSize) => new PagedResponse<BlockLogEntry>
            {
                Page = page,
                PageSize = pageSize,
                Total = _logs.Count,
                Items = _logs.Skip((page - 1) * pageSize).Take(pageSize)
            };
    }
}
