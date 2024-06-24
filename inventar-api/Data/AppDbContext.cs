using inventar_api.ArticleLocations.Models;
using inventar_api.Articles.Models;
using inventar_api.Locations.Models;
using inventar_api.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.Data;

public class AppDbContext: IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<ArticleLocation> ArticleLocations { get; set; }
    public virtual DbSet<ArticleLocationHistory> ArticleLocationHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users"); // Explicit table name assignment
            entity.Property(s => s.Email).IsRequired().HasMaxLength(256);
            entity.Property(s => s.NormalizedEmail).HasMaxLength(256);
            entity.Property(s => s.UserName).IsRequired().HasMaxLength(256);
            entity.Property(s => s.NormalizedUserName).HasMaxLength(256);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Name).IsRequired();
            entity.Property(s => s.Age).IsRequired();
            entity.Property(s => s.Gender).IsRequired();
            
            entity.HasDiscriminator<string>("Discriminator").HasValue("User");
        });
        
        modelBuilder.Entity<ArticleLocation>()
            .HasOne(al => al.Article)
            .WithMany(a => a.ArticleLocations)
            .HasForeignKey(al => al.ArticleCode)
            .HasPrincipalKey(a => a.Code)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ArticleLocation>()
            .HasOne(al => al.Location)
            .WithMany(l => l.ArticleLocations)
            .HasForeignKey(al => al.LocationCode)
            .HasPrincipalKey(l => l.Code)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ArticleLocationHistory>()
            .HasOne(alh => alh.Article)
            .WithMany(a => a.ArticleLocationHistory)
            .HasForeignKey(alh => alh.ArticleCode)
            .HasPrincipalKey(a => a.Code)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ArticleLocationHistory>()
            .HasOne(alh => alh.Location)
            .WithMany(l => l.ArticleLocationHistory)
            .HasForeignKey(alh => alh.LocationCode)
            .HasPrincipalKey(l => l.Code)
            .OnDelete(DeleteBehavior.Cascade);
    }
}