using api_ecommerce.SwaggerConfig;
using Asp.Versioning.ApiExplorer;
using DotNetEnv;
using Swashbuckle.AspNetCore.SwaggerGen;
using Models.Models;
using Infra;
using api_ecommerce.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Env.Load(); // Adiciona vari�veis de ambiente diretamente

// Verifique se a vari�vel foi carregada
string? connectionString = Environment.GetEnvironmentVariable("SQLData");
Console.WriteLine($"SQLData: {connectionString}");
string? secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // Obt�m a chave do .env

var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<UserService>();

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
builder.Services.AddDbContext<UserContext>();

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
