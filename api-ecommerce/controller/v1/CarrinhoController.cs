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
            var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            var carrinho = await _carrinhoService.ObterCarrinhoAsync(usuarioId);
            return Ok(carrinho);
        }

        [HttpPost("adicionar/{produtoId}")]
        [Authorize]
        public async Task<IActionResult> AdicionarAoCarrinho(string produtoId, [FromQuery] int quantidade = 1)
        {
            var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            await _carrinhoService.AdicionarAoCarrinhoAsync(usuarioId, produtoId, quantidade);
            return Ok("Produto adicionado ao carrinho.");
        }

        [HttpDelete("remover/{produtoId}")]
        [Authorize]
        public async Task<IActionResult> RemoverDoCarrinho(string produtoId)
        {
            var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            await _carrinhoService.RemoverDoCarrinhoAsync(usuarioId, produtoId);
            return Ok("Produto removido do carrinho.");
        }

    }
}
