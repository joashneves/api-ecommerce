using Infra;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Models.Models;

namespace api_ecommerce.Services
{
    public class UserService
    {
        private readonly UserContext _context;
        public UserService(UserContext context)
        {
            _context = context;
        }
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.UsuarioSet.ToListAsync(); // Obtém todos os usuários
        }
        public async Task<Usuario> PostUsuarioAsync(UsuarioDTO usuarioDTO)
        {
            var existingUser = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Email == usuarioDTO.Email || u.Nome_usuario == usuarioDTO.Nome_usuario);
            if (existingUser != null)
            {
                throw new ArgumentException("Usuário ou email já existe.");
            }

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var workFactor = environment == "Production" ? 12 : 6;

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(), // Agora criado no service
                Nome_completo = usuarioDTO.Nome_completo,
                Nome_usuario = usuarioDTO.Nome_usuario,
                Email = usuarioDTO.Email,
                CPF = usuarioDTO.CPF,
                Senha = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Senha, workFactor),
                Cargo = Cargo.Comum, // ou outro valor padrão
                Permissoes = Permissao.Ler, // padrão
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                DeletedAt = null
            };
            System.Console.WriteLine(usuario);

            _context.UsuarioSet.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Object> LoginAsync(LoginDTO login)
        {
            var usuario = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Nome_usuario == login.Nome_usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
            {
                throw new ArgumentException("Email ou senha inválidos.");
            }

            return TokenGeneration.GenerateToken(usuario); // já é string

        }

    }
}
