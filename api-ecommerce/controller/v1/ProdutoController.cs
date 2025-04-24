using api_ecommerce.Services;
using Asp.Versioning;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly ProdutoService _produtoContext;

        public ProdutoController(ProdutoService contextService)
        {
            _produtoContext = contextService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _produtoContext.GetProdutosAsync();
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Produto produto){
                if (produto == null)
    {
        return BadRequest("Produto inválido.");
    }

            var result = await _produtoContext.PostProdutoAsync(produto);
            return Created($"api/v1/Produto/{result.Id}", result); // ou apenas: return Ok(result);
        }
    }
}
