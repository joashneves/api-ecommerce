using api_ecommerce.SwaggerConfig;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

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
