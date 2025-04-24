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

        // Método para obter um produto específico pelo ID
        public async Task<Produto> GetProdutoByIdAsync(string id)
        {
            return await _context.ProdutoSet.FirstOrDefaultAsync(p => p.Id == id); // Obtém o produto pelo ID
        }
    }
}
