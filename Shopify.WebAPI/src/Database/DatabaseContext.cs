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
  public DbSet<Product> Products { get; set; }
  public DbSet<Image> Images { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderDetail> OrderDetails { get; set; }

  public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
  {
    _config = config;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var connectionString = _config.GetConnectionString("DB_URL");

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder
      .MapEnum<Role>()
      .MapEnum<Status>();
    var dataSource = dataSourceBuilder.Build();

    optionsBuilder
      .UseNpgsql(dataSource)
      .UseSnakeCaseNamingConvention();

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .HasPostgresEnum<Role>()
      .HasPostgresEnum<Status>();

    modelBuilder.Entity<User>(entity =>
    {
      entity.Property(e => e.Role)
      .HasColumnType("role");

      entity.HasIndex(e => e.Email)
      .IsUnique();

      entity.HasData(SeedingData.GetUsers());
    });

    modelBuilder
      .Entity<Order>()
      .Property(e => e.Status)
      .HasColumnType("status");

    modelBuilder.Entity<Category>(entity =>
    {
      entity
        .HasIndex(e => e.Name)
        .IsUnique();

      entity.HasData(SeedingData.GetCategories());
    });

    modelBuilder.Entity<Product>(entity =>
    {
      entity.HasData(SeedingData.GetProducts());
    });

    modelBuilder.Entity<Image>(entity =>
    {
      entity.HasData(SeedingData.GetImages());
    });

    modelBuilder
      .Entity<OrderDetail>()
      .HasKey(e => new { e.ProductId, e.OrderId });

    base.OnModelCreating(modelBuilder);
  }
}