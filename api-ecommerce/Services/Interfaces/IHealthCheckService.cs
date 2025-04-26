namespace api_ecommerce.Services.Interfaces
{
    public interface IHealthCheckService
    {
        Task<Dictionary<string, string>> CheckDatabaseConnectionsAsync();  // Atualizado para retornar um Dictionary
        Task<object> GetStatusAsync();

    }
}
