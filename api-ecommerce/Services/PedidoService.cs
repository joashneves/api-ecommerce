using Infra;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace api_ecommerce.Services
{
    public class PedidoService
    {
        private readonly ApplicationDbContext _context;
        private readonly CarrinhoService _carrinhoService;

        public PedidoService(ApplicationDbContext context, CarrinhoService carrinhoService)
        {
            _context = context;
            _carrinhoService = carrinhoService;
        }

        public async Task<List<Pedido>> GetPedidosAsync(int pagina = 0, int quantidade = 10)
        {
            return await _context.PedidoSet
                .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                .Include(p => p.Localizacao)
                .Where(p => p.DeletedAt == null)
                .OrderByDescending(p => p.CriadoEm)
                .Skip(pagina)
                .Take(quantidade)
                .AsQueryable()
                .ToListAsync();
        }


        public async Task<Pedido> GetPedidoByIdAsync(Guid id)
        {
            return await _context.PedidoSet
                .Include(p => p.Produtos)
                .Include(p => p.Localizacao)
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
        }

        public async Task EnviarPedidoAsync(Guid pedidoId)
        {
            var pedido = await GetPedidoByIdAsync(pedidoId);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            if (pedido.Atendido)
                throw new Exception("Pedido já foi atendido.");

            pedido.Atendido = true;
            pedido.UpdatedAt = DateTime.UtcNow;

            _context.PedidoSet.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarPedidoAsync(Pedido pedido)
        {
            pedido.UpdatedAt = DateTime.UtcNow;
            _context.PedidoSet.Update(pedido);
            await _context.SaveChangesAsync();
        }

    }

}
