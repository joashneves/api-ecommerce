using Infra;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace api_ecommerce.Services
{
    public class LocalizacaoService
    {
        private readonly LocalizacaoContext _context;

        public LocalizacaoService(LocalizacaoContext context)
        {
            _context = context;
        }

        public async Task CadastrarLocalizacaoAsync(string usuarioId, Localizacao localizacao)
        {
            
            var existente = await _context.LocalizacaoSet.FirstOrDefaultAsync(l => l.UsuarioId == usuarioId && l.DeletedAt == null);
            
            if (existente != null)
            {
                // Atualizar localização existente
                existente.Endereco = localizacao.Endereco;
                existente.Cidade = localizacao.Cidade;
                existente.Estado = localizacao.Estado;
                existente.Cep = localizacao.Cep;
                existente.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            }
            else
            {
                localizacao.UsuarioId = usuarioId;
                await _context.LocalizacaoSet.AddAsync(localizacao);
            }

            await _context.SaveChangesAsync();
        }
    }

}
