using IpBlock.Models;
using IpBlock.Models.DTOs.Response;

namespace IpBlock.Services
{
    public interface ILogService
    {
        void LogAttempt(BlockLogEntry entry);
        PagedResponse<BlockLogEntry> GetBlockedAttempts(int page, int pageSize);
    }
}
