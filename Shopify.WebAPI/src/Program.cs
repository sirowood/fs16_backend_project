using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.WebAPI.src.Middleware;
using Shopify.WebAPI.src.Database;
using Shopify.WebAPI.src.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Shopify.WebAPI.src.Service;
using Shopify.WebAPI.src.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllersWithViews(options => { options.SuppressAsyncSuffixInActionNames = false; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
  .AddScoped<ITokenService, TokenService>()
  .AddScoped<IAuthService, AuthService>()
  .AddScoped<IUserService, UserService>()
  .AddScoped<IUserRepo, UserRepo>()
  .AddScoped<IAddressService, AddressService>()
  .AddScoped<IAddressRepo, AddressRepo>()
  .AddScoped<ICategoryService, CategoryService>()
  .AddScoped<ICategoryRepo, CategoryRepo>()
  .AddScoped<IProductService, ProductService>()
  .AddScoped<IProductRepo, ProductRepo>()
  .AddScoped<IImageService, ImageService>()
  .AddScoped<IImageRepo, ImageRepo>()
  .AddScoped<IOrderService, OrderService>()
  .AddScoped<IOrderRepo, OrderRepo>();

builder.Services
  .AddSingleton<IAuthorizationHandler, OrderOwnerHandler>()
  .AddSingleton<IAuthorizationHandler, OrderOwnerOrAdminHandler>();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql());

builder
  .Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(o =>
  {
    o.TokenValidationParameters = new TokenValidationParameters
    {
      ValidIssuer = builder.Configuration["Jwt:Issuer" ?? "Default Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience" ?? "Default Audience"],
      IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "Default Key")),
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true
    };
  });

builder.Services.AddAuthorization(policy =>
{
  policy.AddPolicy("OrderOwner", policy => policy.Requirements.Add(new OrderOwnerRequirement()));
  policy.AddPolicy("OrderOwnerOrAdmin", policy => policy.Requirements.Add(new OrderOwnerOrAdminRequirement()));
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
