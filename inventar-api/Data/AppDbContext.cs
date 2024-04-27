using inventar_api.ArticleLocations.Models;
using inventar_api.Articles.Models;
using inventar_api.Locations.Models;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<ArticleLocation> ArticleLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}