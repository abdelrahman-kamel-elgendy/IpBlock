using IpBlock.Models;
using IpBlock.Models.DTOs.Response;

namespace IpBlock.Repositories
{
    public interface ICountryRepository
    {
        bool AddBlocked(BlockCountryEntity country);
        bool RemoveBlocked(string countryCode);
        PagedResponse<object> GetAll(int page, int pageSize, string? q);
        bool IsBlocked(string countryCode);
    }
}
