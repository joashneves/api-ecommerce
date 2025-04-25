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
        public async Task<Usuario> PostUsuarioAsync(Usuario usuario)
        {
            var existingUser = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Email == usuario.Email || u.Nome_usuario == usuario.Nome_usuario);
            if (existingUser != null)
            {
                throw new ArgumentException("Usuário ou email já existe.");
            }
            // Checar ambiente
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var workFactor = environment == "Production" ? 12 : 6;
            // Hash da senha com salting interno
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor);

            _context.UsuarioSet.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;

        }
        public async Task<string> LoginAsync(LoginDTO login)
        {
            var usuario = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Nome_usuario == login.Nome_usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
            {
                throw new ArgumentException("Email ou senha inválidos.");
            }

            var token = TokenGeneration.GenerateToken(usuario);
            return (string)token;
        }

    }
}
