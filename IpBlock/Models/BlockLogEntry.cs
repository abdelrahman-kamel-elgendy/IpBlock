namespace IpBlock.Models
{
    public class BlockLogEntry
    {
        public string Ip { get; init; } = string.Empty;
        public string CountryCode { get; init; } = string.Empty;
        public bool IsBlocked { get; init; }
        public string UserAgent { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; } = DateTime.Now;

        public BlockLogEntry(string ip , string countryCode, bool isBlocked, string userAgant)
        {
            Ip = ip;
            CountryCode = countryCode;
            IsBlocked = isBlocked;
            UserAgent = userAgant;
        }
    }
}
