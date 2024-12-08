using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using digeset_server.Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<digesetContext>
{
    public digesetContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<digesetContext>();
        optionsBuilder.UseSqlite("Data Source=digeset.db"); // Cadena de conexión a SQLite

        return new digesetContext(optionsBuilder.Options);
    }
}
