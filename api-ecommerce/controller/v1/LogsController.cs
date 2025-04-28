using api_ecommerce.Attributes;
using api_ecommerce.Services;
using Asp.Versioning;
using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Models.Models;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class LogsController : Controller
    {
        private readonly LogService _logService;

        public LogsController(LogService contextService)
        {
            _logService = contextService;
        }

        [HttpGet]
        [AutorizarCargo(Cargo.Suporte, Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> GetLogs([FromQuery] int pagina = 0, [FromQuery] int quantidade = 10)
        {
            var logs = await _logService.GetLogsAsync(pagina, quantidade);
            return Ok(logs);
        }

    }
}
