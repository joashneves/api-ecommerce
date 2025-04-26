using System.Threading.Tasks;
using api_ecommerce.Attributes;
using api_ecommerce.Services;
using Asp.Versioning;
using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Models.Models;

namespace api_ecommerce.controller.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService contextService)
        {
            _userService = contextService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetUsuariosAsync();
            return Ok(result);
        }
        [HttpPost]
        [AutorizarCargo(Cargo.SuperAdm)]
        [AutorizarPermissao(Attributes.Permissao.GerenciarUsuarios)]
        public async Task<IActionResult> Post([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var result = await _userService.PostUsuarioAsync(usuarioDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Auth([FromBody] LoginDTO login)
        {
            try
            {
                var token = await _userService.LoginAsync(login);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno ao autenticar", erro = ex.Message });
            }
        }
    }
}
