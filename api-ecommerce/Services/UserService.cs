using Infra;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Models.Models;

namespace api_ecommerce.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
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
        public async Task<Usuario> PatchUsuarioAsync(string nomeUsuario, Dictionary<string, object> dadosPatch, string usuarioLogado, Cargo cargoLogado)
        {
            var usuario = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Nome_usuario == nomeUsuario);

            if (usuario == null)
                throw new ArgumentException("Usuário não encontrado.");

            if (usuario.Nome_usuario != usuarioLogado && cargoLogado != Cargo.Adm && cargoLogado != Cargo.SuperAdm)
                throw new UnauthorizedAccessException("Você não tem permissão para alterar este usuário.");

            foreach (var dado in dadosPatch)
            {
                var propriedade = dado.Key.ToLower();
                var valor = dado.Value?.ToString();

                if (string.IsNullOrEmpty(valor))
                    continue;

                switch (propriedade)
                {
                    case "nome_completo":
                        usuario.Nome_completo = valor;
                        break;
                    case "nome_usuario":
                        usuario.Nome_usuario = valor;
                        break;
                    case "email":
                        usuario.Email = valor;
                        break;
                    case "senha":
                        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                        var workFactor = environment == "Production" ? 12 : 6;
                        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(valor, workFactor);
                        break;
                    // Protegendo campos sensíveis:
                    case "id":
                    case "cpf":
                    case "createdat":
                    case "deletedat":
                        // Ignorar esses campos
                        break;
                    default:
                        // Ignorar campos desconhecidos
                        break;
                }
            }

            usuario.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            _context.UsuarioSet.Update(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }


        public async Task<Usuario> PatchUsuarioAdmAsync(string nomeUsuario, AlterarUsuarioCargosPorAdminDTO usuarioAdminDTO)
        {
            var usuario = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Nome_usuario == nomeUsuario);

            if (usuario == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            // Atualiza campos
            if (usuarioAdminDTO.Cargo.HasValue)
                usuario.Cargo = usuarioAdminDTO.Cargo.Value;

            if (usuarioAdminDTO.Permissoes.HasValue)
                usuario.Permissoes = usuarioAdminDTO.Permissoes.Value;

            usuario.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            _context.UsuarioSet.Update(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }


        public async Task<Object> LoginAsync(LoginDTO login)
        {
            var usuario = await _context.UsuarioSet
                .FirstOrDefaultAsync(u => u.Nome_usuario == login.Nome_usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
            {
                throw new ArgumentException("Usuario ou senha inválidos.");
            }

            return TokenGeneration.GenerateToken(usuario); // já é string

        }

    }
}
