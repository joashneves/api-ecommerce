using System;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly ApplicationDbContext _context;
        public ProdutoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obter todos os produtos
        public async Task<List<Produto>> GetProdutosAsync(int pagina = 0, int quantidade = 10)
        {
            if (pagina < 0)
            {
                throw new ArgumentException("A página não pode ser menor que zero.");
            }
            else if (quantidade >= 30)
            {
                throw new ArgumentException("A quantidade não pode ser maior que 30.");
            }
            else if (quantidade <= 0)
            {
                throw new ArgumentException("A quantidade não pode ser menor ou igual a zero.");
            }
            return await _context.ProdutoSet
            .Include(p => p.Imagens)
            .Where(p => p.DeletedAt == null)
            .Skip(pagina).Take(quantidade)
            .AsQueryable().ToListAsync();
        }
        public async Task<Produto> GetProdutoByIdAsync(Guid id)
        {
            var produto = await _context.ProdutoSet
                .Include(p => p.Imagens)
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado.");
            }

            return produto;
        }
        // Metodo Regex
        public async Task<List<Produto>> BuscarProdutosPorRegexAsync(string termo, int pagina = 0, int quantidade = 10)
        {
            if (pagina < 0)
                throw new ArgumentException("A página não pode ser menor que zero.");
            if (quantidade <= 0 || quantidade > 30)
                throw new ArgumentException("A quantidade deve ser entre 1 e 30.");

            // Consulta inicial no banco (mais leve) — Like/Contains
            var produtosFiltrados = await _context.ProdutoSet
                .Include(p => p.Imagens)
                .Where(p => p.DeletedAt == null &&
                       (p.Nome.Contains(termo) || p.Descricao.Contains(termo)))
                .ToListAsync();

            // Aplica regex em memória
            var regex = new System.Text.RegularExpressions.Regex(termo, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var resultadosRegex = produtosFiltrados
                .Where(p => regex.IsMatch(p.Nome) || regex.IsMatch(p.Descricao))
                .Skip(pagina).Take(quantidade)
                .ToList();

            return resultadosRegex;
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
        public async Task<Produto> PatchProdutoAsync(Guid id, Dictionary<string, object> dadosPatch, string usuarioLogado, Cargo cargoLogado)
        {
            var produto = await _context.ProdutoSet
                .Include(p => p.Imagens) // Se quiser atualizar imagens depois
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                throw new ArgumentException("Produto não encontrado.");

            if (cargoLogado != Cargo.Adm && cargoLogado != Cargo.SuperAdm && cargoLogado != Cargo.Suporte)
                throw new UnauthorizedAccessException("Você não tem permissão para alterar este produto.");

            foreach (var dado in dadosPatch)
            {
                var propriedade = dado.Key.ToLower();
                var valor = dado.Value?.ToString();

                if (valor == null)
                    continue;

                switch (propriedade)
                {
                    case "nome":
                        produto.Nome = valor;
                        break;
                    case "descricao":
                        produto.Descricao = valor;
                        break;
                    case "preco":
                        if (decimal.TryParse(valor, out var preco))
                            produto.Preco = preco;
                        break;
                    case "categoria":
                        produto.Categoria = valor;
                        break;
                    case "quantidade":
                        if (int.TryParse(valor, out var quantidade))
                            produto.Quantidade = quantidade;
                        break;
                    // Se quiser tratar imagens no futuro, adicione aqui
                    default:
                        // Ignorar campos desconhecidos
                        break;
                }
            }

            produto.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            _context.ProdutoSet.Update(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
        public async Task<Produto> ComprarProdutoAsync(Guid id, int quantidadeCompra)
        {
            var produto = await _context.ProdutoSet.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                throw new ArgumentException("Produto não encontrado.");

            if (produto.Quantidade < quantidadeCompra)
                throw new InvalidOperationException("Estoque insuficiente para a compra.");

            produto.Quantidade -= quantidadeCompra;
            produto.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            _context.ProdutoSet.Update(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
        public async Task<string> DeletarOuRestaurarProdutoAsync(Guid id)
        {
            var produto = await _context.ProdutoSet.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado.");
            if (produto.DeletedAt == null)
            {
                // Marca como deletado
                produto.DeletedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            }
            else
            {
                // Restaura o produto
                produto.DeletedAt = null;
            }
            _context.ProdutoSet.Update(produto);
            await _context.SaveChangesAsync();
            return produto.DeletedAt == null ? "Produto restaurado com sucesso." : "Produto deletado com sucesso.";
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
