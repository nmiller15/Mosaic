using Microsoft.EntityFrameworkCore;
using Mosaic.Features.Chipper;

namespace Mosaic.Shared;

public class MosaicDbContext : DbContext
{
    public MosaicDbContext(DbContextOptions<MosaicDbContext> options) : base(options)
    {
    }

    public DbSet<LogMessage> LogMessages { get; set; }
    public DbSet<AppSourceSubscriber> AppSourceSubscribers { get; set; }
    public DbSet<AppSource> AppSources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure LogMessage entity
        modelBuilder.Entity<LogMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Level).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MessageTemplate).IsRequired();
            entity.Property(e => e.RenderedMessage);
            entity.Property(e => e.Properties).HasColumnType("jsonb").IsRequired(false);
            entity.Property(e => e.Exception);
        });

        // Configure AppSource entity
        modelBuilder.Entity<AppSource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Seed initial data
            entity.HasData(
                new AppSource { Id = 1, Name = "Investigations", Description = "Investigations app source", CreatedAt = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc) }
            );
        });

        // Configure AppSourceSubscriber entity
        modelBuilder.Entity<AppSourceSubscriber>(entity =>
        {
            entity.HasKey(e => new { e.AppSourceKey, e.Email });
            entity.Property(e => e.AppSourceKey).ValueGeneratedNever();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Level);
        });
    }
}
