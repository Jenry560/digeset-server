using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using digeset_server.Infrastructure.Context;
using Microsoft.Extensions.Configuration;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<digesetContext>
{
    public digesetContext CreateDbContext(string[] args)
    {
        { // Localiza la carpeta raíz del proyecto API
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../digeset-server.Api");

            // Construye el IConfiguration para leer el appsettings.json del proyecto API
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Obtén la cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura las opciones del DbContext
            var optionsBuilder = new DbContextOptionsBuilder<digesetContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new digesetContext(optionsBuilder.Options);
        }
    }
}