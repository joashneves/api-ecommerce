using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Models.Models;

namespace Infra.Seed
{
    public class UsuarioSeed
    {
       
            public static List<Usuario> SeedUsuarios()
            {
                return new List<Usuario>
            {
                new Usuario
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Nome_completo = "Super Admin",
                    Nome_usuario = "superadmin",
                    Senha = "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", // Hash fixo
                    Email = "superadmin@exemplo.com",
                    CPF = "00000000000",
                    Cargo = Cargo.SuperAdm,
                    Permissoes = Permissao.GerenciarUsuarios | Permissao.GerenciarProdutos | Permissao.Criar | Permissao.Excluir | Permissao.Atualizar | Permissao.Ler,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(2025, 04, 26, 0, 0, 0), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                    UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(2025, 04, 26, 0, 0, 0), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                   DeletedAt = null

                },
                new Usuario
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Nome_completo = "Usuário Comum",
                    Nome_usuario = "usuario",
                    Senha = "$2a$06$zuBeb8OtvdVc.ji4HoIkOeQcA7A9SObOSfj.QVbiEebHx6KZKIdgO", // Hash fixo
                    Email = "usuario@exemplo.com",
                    CPF = "11111111111",
                    Cargo = Cargo.Comum,
                    Permissoes = Permissao.Ler,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(2025, 04, 26, 0, 0, 0), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                    UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(2025, 04, 26, 0, 0, 0), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                    DeletedAt = null

                }
            };
            }
        }
    }

