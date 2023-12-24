using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class OrderService : BaseService<Order, OrderReadDTO, OrderCreateDTO, OrderUpdateDTO>, IOrderService
{
  private readonly new IOrderRepo _repo;
  private readonly IProductRepo _productRepo;
  public OrderService(IOrderRepo repo, IProductRepo productRepo, IMapper mapper) : base(repo, mapper)
  {
    _repo = repo;
    _productRepo = productRepo;
  }

  public override async Task<OrderReadDTO> CreateOneAsync(OrderCreateDTO createDTO)
  {
    if (createDTO.OrderDetails.Count == 0)
    {
      throw CustomException.EmptyOrderDetails();
    }

    foreach (var orderDetail in createDTO.OrderDetails)
    {
      if (orderDetail.Quantity < 1)
      {
        throw CustomException.InvalidQuantity();
      }

      if (orderDetail.PriceAtPurchase < 0)
      {
        throw CustomException.InvalidPrice();
      }

      _ = await _productRepo.GetByIdAsync(orderDetail.ProductId)
        ?? throw CustomException.NotFound($"No such product: {orderDetail.ProductId}");
    }

    return await base.CreateOneAsync(createDTO);
  }

  public async Task<IEnumerable<OrderReadDTO>> GetUserOrders(Guid id)
  {
    var orders = await _repo.GetUserOrders(id);

    return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDTO>>(orders);
  }
}