using IpBlock.Exceptions;
using IpBlock.Models.DTOs.Request;
using IpBlock.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpBlock.Controllers
{

    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountriesController(ICountryService service)
        {
            _service = service;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] BlockCountryRequest request) => CreatedAtAction(nameof(GetBlockedCountries), _service.AddBlocked(request));
        
        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode) 
        {
            if (!_service.RemoveBlocked(countryCode))
                throw new InternalServerErrorException("Block not removed!");

            return NoContent();
        }
        
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? q = null) => Ok(_service.GetAll(page, pageSize, q));
    }

}
