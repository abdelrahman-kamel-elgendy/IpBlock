using IpBlock.Models.DTOs.Request;

namespace IpBlock.Models
{
    public class BlockCountryEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public bool IsTemporal { get; set; }
        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public BlockCountryEntity(string countryCode, string name)
        {
            Code = countryCode;
            Name = name;
            IsTemporal = false;
            CreatedAt = DateTime.Now;
        }
    }
}
