using api_ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Asp.Versioning;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class LocalizacaoController : Controller
    {
        private readonly LocalizacaoService _localizacaoService;
        public LocalizacaoController(LocalizacaoService localizacaoService)
        {
            _localizacaoService = localizacaoService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CadastrarLocalizacao([FromBody] Localizacao request)
        {
            var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            await _localizacaoService.CadastrarLocalizacaoAsync(usuarioId, request);
            return Ok("Localização cadastrada/atualizada com sucesso.");
        }
    }
}
