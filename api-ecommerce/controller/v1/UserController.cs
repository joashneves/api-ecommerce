using System.Security.Claims;
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
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetUsuariosAsync();
            return Ok(result);
        }
        [HttpPost("Cadastrar")]
        [AllowAnonymous]
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
        [HttpPatch("{username}")]
        [AutorizarCargo(Cargo.Comum, Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> Patch(string username, [FromBody] Dictionary<string, object> dadosPatch)
        {
            try
            {
                var usuarioLogado = User.FindFirst(ClaimTypes.Name)?.Value;
                var cargoLogadoString = User.FindFirst("Cargo")?.Value;
                System.Console.WriteLine("Usuário: " + usuarioLogado + " | Cargo: " + cargoLogadoString);

                if (string.IsNullOrEmpty(usuarioLogado) || string.IsNullOrEmpty(cargoLogadoString)){
                    return Unauthorized("Token inválido ou expirado.");
                }

                var cargoLogado = Enum.Parse<Cargo>(cargoLogadoString);

                var result = await _userService.PatchUsuarioAsync(username, dadosPatch, usuarioLogado, cargoLogado);
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


        [HttpPatch("adm/{username}")]
        [AutorizarCargo(Cargo.Adm, Cargo.SuperAdm)]
        public async Task<IActionResult> PatchAdm(string username, [FromBody] AlterarUsuarioCargosPorAdminDTO usuarioAdminDTO)
        {
            try
            {
                var result = await _userService.PatchUsuarioAdmAsync(username, usuarioAdminDTO);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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
