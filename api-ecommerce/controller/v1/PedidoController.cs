using api_ecommerce.Attributes;
using api_ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Asp.Versioning;
using Model.Models;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [Authorize]
        [AutorizarCargo(Cargo.Suporte, Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> GetPedidos([FromQuery] int pagina = 0, [FromQuery] int quantidade = 10)
        {
            var pedidos = await _pedidoService.GetPedidosAsync(pagina, quantidade);
            return Ok(pedidos);
        }

        [HttpPost("{id}/enviar")]
        [Authorize]
        [AutorizarCargo(Cargo.Suporte, Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> EnviarPedido(Guid id)
        {
            try
            {
                await _pedidoService.EnviarPedidoAsync(id);
                return Ok("Pedido enviado e marcado como atendido.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        [AutorizarCargo(Cargo.Suporte, Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> AtualizarPedido(Guid id, [FromBody] Pedido pedidoAtualizado)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);

            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            pedido.Acao = pedidoAtualizado.Acao ?? pedido.Acao;
            pedido.LocalizacaoId = pedidoAtualizado.LocalizacaoId != Guid.Empty ? pedidoAtualizado.LocalizacaoId : pedido.LocalizacaoId;
            pedido.ValorTotal = pedidoAtualizado.ValorTotal != 0 ? pedidoAtualizado.ValorTotal : pedido.ValorTotal;
            // Aqui você pode adicionar mais campos que podem ser atualizados!

            await _pedidoService.AtualizarPedidoAsync(pedido);

            return Ok("Pedido atualizado com sucesso.");
        }
    }

    }
