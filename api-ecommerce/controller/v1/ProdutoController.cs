using api_ecommerce.Services;
using Asp.Versioning;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Models.Models;

namespace api_ecommerce.controller.v1
{
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
        public async Task<IActionResult> Index()
        {
            var result = await _produtoService.GetProdutosAsync();
            return Ok(result);

        }
        [HttpPost]
        [Consumes("multipart/form-data")]
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
    }
}
