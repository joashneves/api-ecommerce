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
        [HttpDelete("{id}")]
        [AutorizarPermissao(Attributes.Permissao.Excluir)]
        [AutorizarCargo(Cargo.Adm)]
        public IActionResult DeleteProduto(Guid id)
        {
            // Lógica de exclusão do produto
            return Ok("Produto excluído.");
        }

    }
}
