using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Infra
{
    public class LocalizacaoContext : DbContext
    {
        public LocalizacaoContext() {}
        public LocalizacaoContext(DbContextOptions<LocalizacaoContext> options)
            : base(options)
        {
        }
        public DbSet<Localizacao> LocalizacaoSet { get; set; }
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
            modelBuilder.Entity<Localizacao>(entity =>
            {
                entity.ToTable("Localizacao");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                    .HasColumnName("id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()");
            });
        }
       
    }
}
