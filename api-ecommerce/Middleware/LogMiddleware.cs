using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Infra;
using Model.Models;
using System.Text.Json;

public class LogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LogMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context); // Continua o pipeline
        }
        finally
        {
            stopwatch.Stop();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var log = new Log
                {
                    Level = "Information",
                    Message = "Request handled.",
                    Usuario = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Anônimo",
                    RequestId = context.TraceIdentifier,
                    Ip = context.Connection.RemoteIpAddress?.ToString(),
                    Endpoint = context.Request.Path,
                    Dados = JsonSerializer.Serialize(new
                    {
                        QueryString = context.Request.QueryString.ToString(),
                        Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
                    }),
                    ExecucaoMs = (int)stopwatch.ElapsedMilliseconds,
                    Ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
                };

                dbContext.Logs.Add(log);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
