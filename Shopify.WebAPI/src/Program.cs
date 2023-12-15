using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.WebAPI.src.Middleware;
using Shopify.WebAPI.src.Database;
using Shopify.WebAPI.src.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql());

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
