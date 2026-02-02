using IcMarketsTestTask.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IcMarketsTestTask.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blockcypher> Blockcyphers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

        modelBuilder.Entity<Blockcypher>(entity =>
        {
            entity.ToTable("Blockcypher", "core");
            
            entity.HasKey(x => x.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });
    }
}