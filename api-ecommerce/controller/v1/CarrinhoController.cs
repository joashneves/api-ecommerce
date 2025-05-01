using api_ecommerce.Services;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CarrinhoController : Controller
    {
        private readonly CarrinhoService _carrinhoService;

        public CarrinhoController(CarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterCarrinho()
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdString))
                return Unauthorized("Usuário não autenticado.");

            if (!Guid.TryParse(usuarioIdString, out var usuarioId))
                return BadRequest("ID do usuário inválido.");

            var carrinho = await _carrinhoService.ObterCarrinhoAsync(usuarioId);
            return Ok(carrinho);
        }


        [HttpPost("adicionar/{produtoId}")]
        [Authorize]
        public async Task<IActionResult> AdicionarAoCarrinho(Guid produtoId, [FromQuery] int quantidade = 1)
        {
            var usuarioIdstring = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdstring))
                return Unauthorized("Usuário não autenticado.");

            if (!Guid.TryParse(usuarioIdstring, out var usuarioId))
                return BadRequest("ID do usuário inválido.");


            await _carrinhoService.AdicionarAoCarrinhoAsync(usuarioId, produtoId, quantidade);
            return Ok("Produto adicionado ao carrinho.");
        }

        [HttpDelete("remover/{produtoId}")]
        [Authorize]
        public async Task<IActionResult> RemoverDoCarrinho(Guid produtoId)
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdString))
                return Unauthorized("Usuário não autenticado.");
            if (!Guid.TryParse(usuarioIdString, out var usuarioId))
                return BadRequest("ID do usuário inválido.");


            await _carrinhoService.RemoverDoCarrinhoAsync(usuarioId, produtoId);
            return Ok("Produto removido do carrinho.");
        }

    }
}
