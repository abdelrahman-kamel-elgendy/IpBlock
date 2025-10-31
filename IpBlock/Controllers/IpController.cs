using IpBlock.Exceptions;
using IpBlock.Models.DTOs.Response;
using IpBlock.Repositories;
using IpBlock.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IpBlock.Controllers
{

    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IIpApiService _ipApiService;

        public IpController(IIpApiService ipApiService) => _ipApiService = ipApiService;


        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress = null)
        {
            string ip = string.IsNullOrWhiteSpace(ipAddress) ? GetCallerIp() : ipAddress;

            if (!IPAddress.TryParse(ip, out _))
                throw new AppException("Invalid IP address format.", HttpStatusCode.BadRequest);

            return Ok(await _ipApiService.LookupAsync(ip));
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrWhiteSpace(userAgent))
                userAgent = "Unknown";

            string ip = GetCallerIp();
            if (!IPAddress.TryParse(ip, out _))
                throw new AppException("Invalid IP address format.", HttpStatusCode.BadRequest);
            

            return Ok(await _ipApiService.CheckBlockedAsync(ip, userAgent));
        }


        private string GetCallerIp()
        {
            string? xf = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(xf))
                return xf.Split(',')[0].Trim();

            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        }
    }

}
