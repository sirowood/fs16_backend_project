using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Service;

public class AddressService : BaseService<Address, AddressReadDTO, AddressCreateDTO, AddressUpdateDTO>, IAddressService
{
  public AddressService(IAddressRepo repo, IMapper mapper) : base(repo, mapper)
  {
  }
}