using IpBlock.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpBlock.Controllers
{

    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService) => _logService = logService;

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 20) => Ok(_logService.GetBlockedAttempts(page, pageSize));
    }
}
