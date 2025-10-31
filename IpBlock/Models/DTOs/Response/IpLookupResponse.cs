namespace IpBlock.Models.DTOs.Response
{
    public class IpLookupResponse
    {
        public string Ip { get; set; } = "";
        public string CountryCode { get; set; } = "";
        public string CountryName { get; set; } = "";
        public string? Isp { get; set; }
        public string? userAgent { get; set; }
    }
}
