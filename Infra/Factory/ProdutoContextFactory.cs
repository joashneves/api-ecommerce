using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infra
{
    public class ProdutoContextFactory : IDesignTimeDbContextFactory<ProdutoContext>
    {
        public ProdutoContext CreateDbContext(string[] args)
        {
            // Caminho até o appsettings.json do projeto de startup (api-ecommerce)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../api-ecommerce"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProdutoContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new ProdutoContext(optionsBuilder.Options);
        }
    }
}
