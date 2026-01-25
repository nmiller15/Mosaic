using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Mosaic.Shared;

namespace Mosaic.Configuration;

public class MosaicDbContextFactory : IDesignTimeDbContextFactory<MosaicDbContext>
{
    public MosaicDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MosaicDbContext>();
        
        // Use local development connection string for design-time operations (bundle generation)
        // This is only used during EF Core tool operations, not in runtime application
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Database=mosaic;Password=password");
        
        return new MosaicDbContext(optionsBuilder.Options);
    }
}