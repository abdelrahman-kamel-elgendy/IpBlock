using IpBlock.Models;
using IpBlock.Models.DTOs.Request;
using IpBlock.Models.DTOs.Response;

namespace IpBlock.Services
{
    public interface ICountryService
    {
        BlockCountryEntity AddBlocked(BlockCountryRequest request);
        BlockCountryEntity AddBlocked(TemporalBlockRequest request);
        PagedResponse<object> GetAll(int page, int pageSize, string? q);
        bool IsBlocked(string countryCode);
        bool RemoveBlocked(string countryCode);
    }
}
