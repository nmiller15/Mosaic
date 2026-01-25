using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Mosaic.Shared;
using System.IO;

namespace Mosaic.Configuration;

public class MosaicDbContextFactory : IDesignTimeDbContextFactory<MosaicDbContext>
{
    public MosaicDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MosaicDbContext>();
        
        // Check if we're on EC2 server (has production config access)
        if (File.Exists("/config/mosaic.settings.json"))
        {
            // We're on EC2 server - use production config
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("/config/mosaic.settings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            // We're on GitHub runner - use local development
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Database=mosaic;Password=password");
        }
        
        return new MosaicDbContext(optionsBuilder.Options);
    }
}