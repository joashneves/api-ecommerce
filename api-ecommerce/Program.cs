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
using Microsoft.OpenApi.Models;
using api_ecommerce.Services.Interfaces;
using api_ecommerce.Middleware;

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
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<IHealthCheckService, HealthCheckService>();


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
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<SweggerDefaultValue>();

    // CONFIGURAÇÃO DE TOKEN BEARER
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Digite: Bearer {seu token JWT}"
    });

    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Para migrations
builder.Services.AddDbContext<ProdutoContext>();
builder.Services.AddDbContext<UserContext>();
builder.Services.AddDbContext<ApplicationDbContext>();

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

app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<LogMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
