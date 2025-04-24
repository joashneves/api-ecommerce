using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Models;

namespace Infra
{
    public class ProdutoContext : DbContext
    {
        private IConfiguration _configuration;

        // DbSet para acessar os produtos no banco de dados
        public DbSet<Produto> ProdutoSet { get; set; } // Alterado de ContaSet para ProdutoSet

        public ProdutoContext(IConfiguration configuration, DbContextOptions<ProdutoContext> options)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Configuração da string de conexão e do banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Comentado o código de teste para exibir a string de conexão no console.
            // Utilizando SQL Server, substitua a string de conexão conforme necessário.
            string? connectionString = Environment.GetEnvironmentVariable("SQLData"); // Variável de ambiente
            if (string.IsNullOrEmpty(connectionString))
            {
                // Se não encontrar a variável de ambiente, use uma string de conexão padrão
                connectionString = _configuration.GetConnectionString("SQLData"); // A configuração do appsettings.json
                Console.WriteLine("connectionString: " + connectionString);
            }
            // Usando SQL Server (substitua a string de conexão conforme necessário)
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
