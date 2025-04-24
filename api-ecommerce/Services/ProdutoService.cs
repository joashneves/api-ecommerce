using System;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Model.ViewModel;
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
        public async Task<Produto> PostProdutoAsync([FromForm]ProdutoViewModel produto)
        {
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(produto.Arquivo.FileName).ToLowerInvariant();
                if (!permittedExtensions.Contains(ext))
                {
                    throw new ArgumentException("Formato de imagem não permitido.");
                }


                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(produto.Arquivo.FileName);
                var fileName = fileNameWithoutExtension;

            string produtoDir = Path.Combine("Storage", "Produtos");

            if (Directory.Exists(produtoDir) == false)
                {
                    Directory.CreateDirectory(produtoDir);
                    
                }
                var filePath = Path.Combine(produtoDir, produto.Arquivo.FileName);
            System.Console.WriteLine(filePath);
                int count = 1;
                while (System.IO.File.Exists(filePath))
                {
                    System.Console.WriteLine("Arquivo ja existe");
                    fileName = $"{fileNameWithoutExtension}_{count++}{ext}";
                    filePath = Path.Combine(produtoDir, fileName);
                }
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await produto.Arquivo.CopyToAsync(fileStream);
                }
                var Novoproduto = new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Categoria = produto.Categoria,
                    Quantidade = produto.Quantidade,
                    CaminhoArquivo = filePath
                };
                _context.ProdutoSet.Add(Novoproduto); // Adiciona o novo produto ao DbSet
                await _context.SaveChangesAsync(); // Salva as mudanças no banco
                return Novoproduto; // Retorna o produto criado
        }

    }
}
