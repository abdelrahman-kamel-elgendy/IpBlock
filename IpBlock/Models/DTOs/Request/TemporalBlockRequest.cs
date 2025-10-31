using System.ComponentModel.DataAnnotations;

namespace IpBlock.Models.DTOs.Request
{
    public class TemporalBlockRequest : BlockCountryRequest
    {
        [Required(ErrorMessage = "Duration in minutes is required!")]
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes (24 hours)!")]
        public int DurationMinutes { get; set; }
    }
}
