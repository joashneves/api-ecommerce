using System.Security.Claims;
using api_ecommerce.Attributes;
using api_ecommerce.Services;
using Asp.Versioning;
using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Models.Models;

namespace api_ecommerce.controller.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService contextService)
        {
            _produtoService = contextService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] int pagina = 0, [FromQuery] int quantidade = 10)
        {
            var result = await _produtoService.GetProdutosAsync(pagina, quantidade);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var produto = await _produtoService.GetProdutoByIdAsync(id);
                return Ok(produto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("download/{fileName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var (fileBytes, mimeType, downloadName) = await _produtoService.DownloadImageByPath(fileName);
                return File(fileBytes, mimeType, downloadName);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno ao realizar o download.", erro = ex.Message });
            }
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [AutorizarCargo(Cargo.Adm, Cargo.SuperAdm)]
        [AutorizarPermissao(Attributes.Permissao.Criar)]
        public async Task<IActionResult> Post([FromForm]ProdutoViewModel produto){
            try
            {
                var result = await _produtoService.PostProdutoAsync(produto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno ao salvar produto", erro = ex.Message });
            }
        }
        [HttpPatch("{id}")]
        [AutorizarCargo(Cargo.Adm, Cargo.SuperAdm, Cargo.Suporte)]
        public async Task<IActionResult> PatchProduto(Guid id, [FromBody] Dictionary<string, object> dadosPatch)
        {
            try
            {
                var usuarioLogado = User.FindFirst(ClaimTypes.Name)?.Value;
                var cargoLogadoString = User.FindFirst("Cargo")?.Value;

                if (string.IsNullOrEmpty(usuarioLogado) || string.IsNullOrEmpty(cargoLogadoString))
                    return Unauthorized(new { message = "Token inválido ou expirado." });

                var cargoLogado = Enum.Parse<Cargo>(cargoLogadoString);

                var result = await _produtoService.PatchProdutoAsync(id, dadosPatch, usuarioLogado, cargoLogado);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPatch("{id}/comprar")]
        public async Task<IActionResult> ComprarProduto(Guid id, [FromBody] int quantidadeCompra)
        {
            try
            {
                if (quantidadeCompra <= 0)
                    return BadRequest(new { message = "A quantidade deve ser maior que zero." });

                var produtoAtualizado = await _produtoService.ComprarProdutoAsync(id, quantidadeCompra);
                return Ok(produtoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [AutorizarPermissao(Attributes.Permissao.Excluir)]
        [AutorizarCargo(Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            try
            {
                var mensagem = await _produtoService.DeletarOuRestaurarProdutoAsync(id);
                return Ok(new { message = mensagem });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
