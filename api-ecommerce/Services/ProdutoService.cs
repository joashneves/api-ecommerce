using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace api_ecommerce.Services
{
    public class ProdutoService
    {
        private readonly ProdutoContext _context;
        public ProdutoService(ProdutoContext context)
        {
            _context = context;
        }

        // Método para obter todos os produtos
        public async Task<List<Produto>> GetProdutosAsync()
        {
            return await _context.ProdutoSet.ToListAsync(); // Obtém todos os produtos
        }
        // Método para adicionar um novo produto
        public async Task<Produto> PostProdutoAsync(Produto produto)
        {
            _context.ProdutoSet.Add(produto); // Adiciona o novo produto ao DbSet
            await _context.SaveChangesAsync(); // Salva as mudanças no banco
            return produto; // Retorna o produto criado
        }

    }
}
