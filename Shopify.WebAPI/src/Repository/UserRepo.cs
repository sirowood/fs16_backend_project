using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class UserRepo : IUserRepo
{
  private readonly DbSet<User> _users;
  private readonly DatabaseContext _database;
  private readonly IConfiguration _config;

  public UserRepo(DatabaseContext database, IConfiguration config)
  {
    _database = database;
    _config = config;
    _users = database.Users;
  }
  public User CreateOne(User user)
  {
    _users.Add(user);
    _database.SaveChanges();
    return user;
  }

  public bool DeleteOne(Guid id)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<User> GetAll(GetAllOptions options)
  {
    throw new NotImplementedException();
  }

  public User? GetByEmail(string email)
  {
    return _users.FirstOrDefault(u => u.Email == email);
  }

  public User GetById(Guid id)
  {
    throw new NotImplementedException();
  }

  public User UpdateOne(User updateObject)
  {
    throw new NotImplementedException();
  }
}