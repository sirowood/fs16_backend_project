using AutoMapper;
using Moq;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Test.src;

public class OrderServiceTest
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();

  [Fact]
  public async Task CreateOneAsync_ValidOrder_ReturnsOrderReadDTO()
  {
    // Arrange
    var product1 = new Product
    {
      Id = Guid.NewGuid(),
      Title = "Product",
      Description = "description",
      Price = 19.9m,
      CategoryId = Guid.NewGuid()
    };

    var product2 = new Product
    {
      Id = Guid.NewGuid(),
      Title = "Product",
      Description = "description",
      Price = 19.9m,
      CategoryId = Guid.NewGuid()
    };

    var createDto = new OrderCreateDTO
    {
      AddressId = Guid.NewGuid(),
      OrderDetails = new List<OrderDetailCreateDTO>
      {
        new() { ProductId = product1.Id, Quantity = 2, PriceAtPurchase = 10.99m },
        new() { ProductId = product2.Id, Quantity = 1, PriceAtPurchase = 15.99m }
      }
    };

    var order = _mapper.Map<OrderCreateDTO, Order>(createDto);

    var repoMock = new Mock<IOrderRepo>();
    repoMock
      .Setup(repo => repo.CreateOneAsync(It.IsAny<Order>()))
      .ReturnsAsync(order);

    var productRepoMock = new Mock<IProductRepo>();
    productRepoMock
      .Setup(repo => repo.GetByIdAsync(product1.Id))
      .ReturnsAsync(product1);
    productRepoMock
      .Setup(repo => repo.GetByIdAsync(product2.Id))
      .ReturnsAsync(product2);

    var orderService = new OrderService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act
    var result = await orderService.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Order>()), Times.Once);
    productRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(createDto.OrderDetails.Count));
  }

  [Fact]
  public async Task CreateOneAsync_EmptyOrderDetails_ThrowsCustomException()
  {
    // Arrange
    var createDto = new OrderCreateDTO
    {
      AddressId = Guid.NewGuid(),
      OrderDetails = new List<OrderDetailCreateDTO>()
    };

    var repoMock = new Mock<IOrderRepo>();
    var productRepoMock = new Mock<IProductRepo>();

    var orderService = new OrderService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await orderService.CreateOneAsync(createDto));

    // Assert
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Order>()), Times.Never);
    productRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
  }

  [Fact]
  public async Task GetUserOrders_ValidUserId_ReturnsIEnumerableOrderReadDTO()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var order1CreateDto = new OrderCreateDTO
    {
      AddressId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      OrderDetails = new List<OrderDetailCreateDTO>()
    };
    var order2CreateDto = new OrderCreateDTO
    {
      AddressId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      OrderDetails = new List<OrderDetailCreateDTO>()
    };

    var order1 = _mapper.Map<OrderCreateDTO, Order>(order1CreateDto);
    var order2 = _mapper.Map<OrderCreateDTO, Order>(order2CreateDto);

    var orders = new List<Order> { order1, order2 };

    var repoMock = new Mock<IOrderRepo>();
    repoMock
      .Setup(repo => repo.GetUserOrders(userId))
      .ReturnsAsync(orders);

    var productRepoMock = new Mock<IProductRepo>();

    var orderService = new OrderService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act
    var result = await orderService.GetUserOrders(userId);

    // Assert
    Assert.NotNull(result);
    Assert.IsAssignableFrom<IEnumerable<OrderReadDTO>>(result);
    repoMock.Verify(repo => repo.GetUserOrders(userId), Times.Once);
  }
}