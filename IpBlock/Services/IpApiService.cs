using IpBlock.Exceptions;
using IpBlock.Models;
using IpBlock.Models.DTOs.Response;
using IpBlock.Options;
using IpBlock.Repositories;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace IpBlock.Services
{
    public class IpApiService : IIpApiService
    {
        private readonly ICountryService _countryService;
        private readonly ILogService _logService;
        private readonly HttpClient _httpClient;
        private readonly IpApiOptions _options;
        private readonly ILogger<IpApiService> _logger;

        public IpApiService(ICountryService countryService, ILogService logService, HttpClient httpClient, IOptions<IpApiOptions> options, ILogger<IpApiService> logger)
        {
            _countryService = countryService;
            _logService = logService;
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IpLookupResponse> LookupAsync(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new AppException("IP address cannot be empty.", HttpStatusCode.BadRequest);

            if (ip == "::1" || ip == "127.0.0.1")
            {
                return new IpLookupResponse
                {
                    Ip = ip,
                    CountryCode = "LOCAL",
                    CountryName = "Localhost",
                    Isp = "Local Network"
                };
            }

            var baseUrl = _options.BaseUrl.EndsWith('/')
                ? _options.BaseUrl
                : _options.BaseUrl + "/";

            var url = $"{baseUrl}{ip}/json/";
            _logger.LogInformation("Calling IP API: {Url}", url);

            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new AppException("Geo lookup rate limit reached. Try again later.", HttpStatusCode.TooManyRequests);

            if (!response.IsSuccessStatusCode)
                throw new AppException($"Geo lookup failed: {response.StatusCode}", response.StatusCode);

            var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            return new IpLookupResponse
            {
                Ip = ip,
                CountryCode = root.TryGetProperty("country", out var cc) ? cc.GetString() ?? "" : "",
                CountryName = root.TryGetProperty("country_name", out var cn) ? cn.GetString() ?? "" : "",
                Isp = root.TryGetProperty("org", out var org) ? org.GetString() : null
            };
        }

        public async Task<BlockLogEntry> CheckBlockedAsync(string ip, string userAgent)
        {
            var lookupResponse = await LookupAsync(ip);
            bool isBlocked = _countryService.IsBlocked(lookupResponse.CountryCode);
            BlockLogEntry entry = new BlockLogEntry(ip, lookupResponse.CountryCode, isBlocked, userAgent);

            _logService.LogAttempt(entry);
            return entry;
        }
    }
}

