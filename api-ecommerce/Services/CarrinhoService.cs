using Infra;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace api_ecommerce.Services
{
    public class CarrinhoService
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Carrinho> ObterCarrinhoAsync(Guid usuarioId)
        {
            var carrinho = _context.CarrinhoSet.Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(c => c.UsuarioId == usuarioId);

            if (carrinho == null)
            {
                carrinho = new Carrinho { UsuarioId = usuarioId };
                _context.CarrinhoSet.Add(carrinho);
                await _context.SaveChangesAsync();
            }

            return carrinho;
        }

        public async Task AdicionarAoCarrinhoAsync(Guid usuarioId, Guid produtoId, int quantidade)
        {
            var carrinho = await ObterCarrinhoAsync(usuarioId);

            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                var produto =  await _context.ProdutoSet.FindAsync(produtoId);
                if (produto != null)
                {
                    carrinho.Itens.Add(new CarrinhoItem
                    {
                        ProdutoId = produtoId,
                        Quantidade = quantidade
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoverDoCarrinhoAsync(Guid usuarioId, Guid produtoId)
        {
            var carrinho = await ObterCarrinhoAsync(usuarioId);
            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item != null)
            {
                carrinho.Itens.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
