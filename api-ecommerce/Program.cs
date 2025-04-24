using api_ecommerce.SwaggerConfig;
using Asp.Versioning.ApiExplorer;
using DotNetEnv;
using Swashbuckle.AspNetCore.SwaggerGen;
using Models.Models;
using Infra;
using api_ecommerce.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load(); // Adiciona vari�veis de ambiente diretamente

// Verifique se a vari�vel foi carregada
string? connectionString = Environment.GetEnvironmentVariable("SQLData");
Console.WriteLine($"SQLData: {connectionString}");

builder.Services.AddScoped<ProdutoService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<SweggerDefaultValue>();
});

builder.Services.AddApiVersioning().AddMvc().AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

// Para migrations
builder.Services.AddDbContext<ProdutoContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        var version = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach(var description in version.ApiVersionDescriptions)
        {
            option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"e-commerce {description.GroupName.ToUpper()}");
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
