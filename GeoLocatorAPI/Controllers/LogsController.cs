using GeoLocator.Application.Dtos;
using GeoLocator.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoLocatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptsRepository _blockedAttemptsRepository;

        private readonly ILogger<LogsController> _logger;
        public LogsController(
            IBlockedAttemptsRepository blockedAttemptsRepository,
            ILogger<LogsController> logger)
        {
            _blockedAttemptsRepository = blockedAttemptsRepository;
            _logger = logger;
        }


        [HttpGet("blocked-attempts")]
        public async Task<IActionResult> GetBlockedAttempts([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _blockedAttemptsRepository.GetBlockedAttemptsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blocked attempts");
                return StatusCode(500, new { error = "An error occurred while retrieving blocked attempts" });
            }
        }
    }
}
