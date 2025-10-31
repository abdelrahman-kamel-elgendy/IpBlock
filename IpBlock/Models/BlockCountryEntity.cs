using IpBlock.Models.DTOs.Request;

namespace IpBlock.Models
{
    public class BlockCountryEntity
    {
        public String Code { get; set; } = string.Empty;
        public String Name { get; set; } = string.Empty;

        public BlockCountryEntity(BlockCountryRequest request)
        {
            Code = request.CountryCode;
            Name = request.Name;
        }
    }
}
