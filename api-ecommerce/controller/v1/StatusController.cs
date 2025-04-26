using api_ecommerce.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class StatusController : Controller
    {
        private readonly IHealthCheckService _healthCheckService;

        public StatusController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            var dbStatuses = await _healthCheckService.CheckDatabaseConnectionsAsync();
            var status = await _healthCheckService.GetStatusAsync();

            return Ok(new
            {
                status = "Online",
                timestamp = DateTime.UtcNow,
                database = dbStatuses, // Inclui o status das conexões com os DBs
                additional_info = status  // Inclui outras informações de status
            });
        }
    }
}
