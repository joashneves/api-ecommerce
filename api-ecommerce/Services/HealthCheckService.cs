using api_ecommerce.Services.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace api_ecommerce.Services
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly ProdutoContext _produtosContext;
        private readonly UserContext _usuariosContext;

        public HealthCheckService(ProdutoContext produtosContext, UserContext usuariosContext)
        {
            _produtosContext = produtosContext;
            _usuariosContext = usuariosContext;
        }

        public async Task<object> GetStatusAsync()
        {
            await using var conn = _produtosContext.Database.GetDbConnection();
            await conn.OpenAsync();

            var command = conn.CreateCommand();
            command.CommandText = "SELECT version(), (SELECT COUNT(*) FROM pg_stat_activity) AS opened_connections, (SELECT setting::int FROM pg_settings WHERE name = 'max_connections') AS max_connections";
            await using var reader = await command.ExecuteReaderAsync();

            string version = "";
            int opened = 0;
            int max = 0;

            if (await reader.ReadAsync())
            {
                version = reader.GetString(0);
                opened = reader.GetInt32(1);
                max = reader.GetInt32(2);
            }

            return new
            {
                updated_at = DateTime.UtcNow,
                dependencies = new
                {
                    database = new
                    {
                        version,
                        max_connections = max,
                        opened_connection = opened
                    }
                }
            };
        }

        public async Task<Dictionary<string, string>> CheckDatabaseConnectionsAsync()
        {
            var status = new Dictionary<string, string>
            {
                ["produtosDb"] = await TestConnection(_produtosContext),
                ["usuariosDb"] = await TestConnection(_usuariosContext)
            };

            return status;
        }

        private async Task<string> TestConnection(DbContext context)
        {
            try
            {
                return await context.Database.CanConnectAsync() ? "Conectado" : "Falha";
            }
            catch
            {
                return "Erro";
            }
        }
    }
}
