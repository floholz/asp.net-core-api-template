using asp.net_core_api_template.Models;
using asp.net_core_api_template.Models.Authentication;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_api_template.Database;

public class PostgresContext : DbContext
{
    public PostgresContext (DbContextOptions<PostgresContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
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
            entity
                .HasOne(e => e.CreatedBy)
                .WithMany(e => e.CreatedUsers)
                .HasForeignKey(e => e.CreatedById);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
            entity
                .HasOne(e => e.UpdatedBy)
                .WithMany(e => e.UpdatedUsers)
                .HasForeignKey(e => e.UpdatedById);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DeletedAt).HasColumnType("timestamp with time zone");
            entity
                .HasOne(e => e.DeleteBy)
                .WithMany(e => e.DeletedUsers)
                .HasForeignKey(e => e.DeleteById);
        });
    }
}