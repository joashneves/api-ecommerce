using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Models.Models;

namespace api_ecommerce.Services
{
    public class TokenGeneration
    {
        public static object GenerateToken(Usuario conta)
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // Obtém a chave do .env
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, conta.Id.ToString()), // <-- ID aqui
                new Claim(ClaimTypes.Name, conta.Nome_usuario),
                new Claim(ClaimTypes.Email, conta.Email),
                new Claim("Cargo", conta.Cargo.ToString()),
                new Claim("Permissoes", ((int)conta.Permissoes).ToString())

            }),
                Expires = DateTime.UtcNow.AddHours(6), // O token vai expirar em 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);

        }
    }
}
