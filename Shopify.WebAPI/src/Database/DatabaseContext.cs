using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shopify.Core.src.Entity;

namespace Shopify.WebAPI.src.Database;

public class DatabaseContext : DbContext
{
  private readonly IConfiguration _config;
  public DbSet<User> Users { get; set; }
  public DbSet<Address> Addresses { get; set; }
  public DbSet<Category> Categories { get; set; }

  public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
  {
    _config = config;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(_config.GetConnectionString("DatabaseURL"));
    dataSourceBuilder.MapEnum<Role>();
    var dataSource = dataSourceBuilder.Build();

    optionsBuilder
      .UseNpgsql(dataSource)
      .UseSnakeCaseNamingConvention();

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .HasPostgresEnum<Role>();

    modelBuilder.Entity<User>(entity =>
    {
      entity.Property(e => e.Role)
      .HasColumnType("role");

      entity.HasIndex(e => e.Email)
      .IsUnique();
    });

    modelBuilder
      .Entity<Category>()
      .HasIndex(e => e.Name)
      .IsUnique();

    base.OnModelCreating(modelBuilder);
  }
}