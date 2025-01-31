using digeset_server.Application.Mapping;
using digeset_server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

// Cargar variables de entorno desde el archivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Reemplazar las variables en la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString!
    .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
    .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
    .Replace("${DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME"))
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Respeta PascalCase
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<digesetContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
