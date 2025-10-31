using IpBlock.Models;
using IpBlock.Models.DTOs.Response;

namespace IpBlock.Repositories
{
    public interface ILogRepository
    {
        void Add(BlockLogEntry entry);
        PagedResponse<BlockLogEntry> GetBlockedAttempts(int page, int pageSize);
    }
}
