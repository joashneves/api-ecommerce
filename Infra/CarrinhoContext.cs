using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Infra
{
    public class CarrinhoContext : DbContext
    {
        public CarrinhoContext() { }
        public CarrinhoContext(DbContextOptions<CarrinhoContext> options)
            : base(options)
        {
        }
        public DbSet<Carrinho> CarrinhoSet { get; set; }
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
            modelBuilder.Entity<Carrinho>(entity =>
            {
                entity.ToTable("Carrinho");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                    .HasColumnName("id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()");
            });
        }
    }
}
