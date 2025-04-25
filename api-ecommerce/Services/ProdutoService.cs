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
            return await _context.ProdutoSet
                .Include(p => p.Imagens) // Inclui as imagens associadas ao produto
                .Where(p => p.DeletedAt == null) // Opcional: Filtra produtos onde DeletedAt é null, se desejar
                .ToListAsync();
        }
        // Método para adicionar um novo produto
        public async Task<Produto> PostProdutoAsync([FromForm] ProdutoViewModel produto)
        {
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var produtoDir = Path.Combine("Storage", "Produtos");

            if (!Directory.Exists(produtoDir))
            {
                Directory.CreateDirectory(produtoDir);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            var savedFilePaths = new List<string>(); // lista para armazenar os caminhos das imagens salvas

            try
            {
                // Criação do produto (ainda sem imagens)
                var novoProduto = new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Categoria = produto.Categoria,
                    Quantidade = produto.Quantidade
                };

                _context.ProdutoSet.Add(novoProduto);
                await _context.SaveChangesAsync();

                // Salva as imagens associadas
                foreach (var imagem in produto.Imagens)
                {
                    var ext = Path.GetExtension(imagem.FileName).ToLowerInvariant();
                    if (!permittedExtensions.Contains(ext))
                    {
                        throw new ArgumentException("Formato de imagem não permitido.");
                    }
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imagem.FileName);
                    var fileName = fileNameWithoutExtension + ext;
                    var filePath = Path.Combine(produtoDir, fileName);
                    int count = 1;

                    while (File.Exists(filePath))
                    {
                        fileName = $"{fileNameWithoutExtension}_{count++}{ext}";
                        filePath = Path.Combine(produtoDir, fileName);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(fileStream);
                    }

                    savedFilePaths.Add(filePath); // salva o caminho para remoção futura em caso de erro

                    var imagemProduto = new ImagemProduto
                    {
                        Caminho = filePath,
                        ProdutoId = novoProduto.Id
                    };

                    _context.Set<ImagemProduto>().Add(imagemProduto);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return novoProduto;
            }
            catch
            {
                // Remove as imagens já salvas
                foreach (var path in savedFilePaths)
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }

                await transaction.RollbackAsync();
                throw; // relança o erro para tratamento externo
            }
        }


        public async Task<(byte[] fileBytes, string mimeType, string fileName)> DownloadImageByPath(string path)
        {
            //System.Console.WriteLine(path); Storage\\Produtos\\Produto1.jpg é o caminho, ja que o path pega só o arquivo
            string fullPath = Path.Combine("Storage", "Produtos", path);

            // Verifica se o caminho existe no banco (tabela ImagemProduto)
            var imagem = await _context.Set<ImagemProduto>()
                .FirstOrDefaultAsync(img => img.Caminho == fullPath);

            if (imagem == null)
            {
                throw new ArgumentException("Produto não encontrado.");
            }
            // Lê o arquivo em bytes
            byte[] fileBytes = await File.ReadAllBytesAsync(fullPath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Arquivo não encontrado.");
            }


            // Determina o tipo MIME do arquivo com base na extensão
            var mimeType = GetMimeType(fullPath);

            // Retorna o arquivo para download
            return (fileBytes, mimeType, Path.GetFileName(fullPath));

        }
        // Método auxiliar para determinar o tipo MIME com base na extensão do arquivo
        private string GetMimeType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream", // Default para arquivos não reconhecidos
            };
        }
    }
}
