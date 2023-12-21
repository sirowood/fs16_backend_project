using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class AddressRepo : BaseRepo<Address>, IAddressRepo
{

  public AddressRepo(DatabaseContext database) : base(database)
  {
  }
}