using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Models;

namespace Infra
{
    public class ProdutoContext : DbContext
    {
        public ProdutoContext() { }

        // DbSet para acessar os produtos no banco de dados

        public ProdutoContext(DbContextOptions<ProdutoContext> options)
            : base(options)
        {
        }
        public DbSet<Produto> ProdutoSet { get; set; } // Alterado de ContaSet para ProdutoSet

        // Configuração da string de conexão e do banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Comentado o código de teste para exibir a string de conexão no console.
            // Utilizando SQL Server, substitua a string de conexão conforme necessário.
            string? connectionString = Environment.GetEnvironmentVariable("SQLData");
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback (evita erro se a variável não estiver setada)
                connectionString = "Server=localhost;Port=5432;Database=local_db;User Id=local_user;Password=local_password;"; // Ajuste conforme seu ambiente
            }

            // Usando SQL Server (substitua a string de conexão conforme necessário)
            optionsBuilder.UseNpgsql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("varchar") // ou "text", dependendo do seu banco
                .IsRequired();


                entity.Property(p => p.Nome)

                .HasColumnName("Nome");

            });

        }
    }
    }
