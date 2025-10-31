using System.ComponentModel.DataAnnotations;

namespace IpBlock.Models.DTOs.Request
{
    public class BlockCountryRequest
    {
        [Required(ErrorMessage = "Country Code is required!")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country Code must be exactly 2 characters!")]
        [RegularExpression(@"^(?!([A-Z])\1$)[A-Z]{2}$", ErrorMessage = "Country Code must be 2 different uppercase letters!")]
        public string CountryCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
