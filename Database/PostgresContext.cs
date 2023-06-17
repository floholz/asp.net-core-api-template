using asp.net_core_api_template.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_api_template.Database;

public class PostgresContext : DbContext
{
    public PostgresContext (DbContextOptions<PostgresContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
            entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");
        });
    }
}