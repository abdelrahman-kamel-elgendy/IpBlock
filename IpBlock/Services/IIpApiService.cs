using IpBlock.Models;
using IpBlock.Models.DTOs.Response;

namespace IpBlock.Services
{
    public interface IIpApiService
    {
        Task<IpLookupResponse> LookupAsync(string ip);
        Task<BlockLogEntry> CheckBlockedAsync(string ip, String userAgent);
    }
}
