using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Seed;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Models.Models;

namespace Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Carrinho> CarrinhoSet { get; set; }
        public DbSet<Localizacao> LocalizacaoSet { get; set; }
        public DbSet<Pedido> PedidoSet { get; set; }
        public DbSet<Usuario> UsuarioSet { get; set; }
        public DbSet<Produto> ProdutoSet { get; set; }
        public DbSet<ImagemProduto> ImagemProdutoSet { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLData");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Port=5432;Database=local_db;User Id=local_user;Password=local_password;"; // Ajuste conforme seu ambiente
            }
            optionsBuilder.UseNpgsql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto");
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                    .HasColumnName("id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()"); // Requer extensão pgcrypto ou uuid-ossp
                entity.Property(u => u.Nome_completo).HasColumnName("nome_completo");
                entity.Property(u => u.Nome_usuario).HasColumnName("nome_usuario");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.Senha).HasColumnName("senha");
            });
            modelBuilder.Entity<Usuario>().HasData(UsuarioSeed.SeedUsuarios());

        }
    }
}
