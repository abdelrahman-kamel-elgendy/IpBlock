
using IpBlock.Models;
using IpBlock.Models.DTOs.Request;
using IpBlock.Models.DTOs.Response;
using System.Collections.Concurrent;

namespace IpBlock.Repositories
{

    public class CountryRepository : ICountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockCountryEntity> _blocked = new();


        public bool AddBlocked(BlockCountryEntity country) =>_blocked.TryAdd(country.Code, country);

        public PagedResponse<object> GetAll(int page, int pageSize, string? q)
        {
            var query = _blocked.Select(kv => kv.Value).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(c => c.Code.Contains(q.Trim(), StringComparison.OrdinalIgnoreCase) || c.Name.Contains(q.Trim(), StringComparison.OrdinalIgnoreCase));

            var orderedQuery = query.OrderBy(c => c.Code);

            return new PagedResponse<object>
            {
                Page = page,
                PageSize = pageSize,
                Total = orderedQuery.LongCount(),
                Items = orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToArray()
            };
        }
        public bool RemoveBlocked(string countryCode) => _blocked.TryRemove(countryCode, out _);
        public bool IsBlocked(string countryCode)
        {
            if (_blocked.TryGetValue(countryCode, out BlockCountryEntity? country))
            {
                if (country.IsTemporal && country.ExpiresAt <= DateTime.Now)
                {
                    RemoveBlocked(countryCode);
                    return false;
                }
                return true;
            }

            return false;
        }

        public IEnumerable<String> GetExpiredTemporalBlocks() => _blocked.Where(kv => kv.Value.ExpiresAt <= DateTimeOffset.UtcNow && kv.Value.IsTemporal).Select(kv => kv.Key);
    }

}
