using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace api_ecommerce.controller.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Topper");
        }
    }
}
