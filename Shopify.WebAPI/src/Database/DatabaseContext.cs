using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shopify.Core.src.Entity;

namespace Shopify.WebAPI.src.Database;

public class DatabaseContext : DbContext
{
  private readonly IConfiguration _config;
  public DbSet<User> Users { get; set; }

  public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
  {
    _config = config;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost;Port=5434;Database=shopify;Username=admin;Password=admin");
    dataSourceBuilder.MapEnum<Role>();
    var dataSource = dataSourceBuilder.Build();

    optionsBuilder
      .UseNpgsql(dataSource)
      .UseSnakeCaseNamingConvention();

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<Role>();
    modelBuilder.Entity<User>(entity => entity.Property(e => e.Role).HasColumnType("role"));

    modelBuilder.Entity<Address>().HasOne<User>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);

    base.OnModelCreating(modelBuilder);
  }
}