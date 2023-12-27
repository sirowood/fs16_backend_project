using AutoMapper;
using Moq;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Test.src;


// AddressService is purly inherit from the BaseService,
// So this test is for the both.
public class AddressServiceTest
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();

  [Fact]
  public async Task CreateOneAsync_ValidAddress_ReturnsAddressReadDTO()
  {
    // Arrange
    var createDto = new AddressCreateDTO
    {
      PostCode = "00100",
      Street = "123 Main St",
      City = "City",
      Country = "Country"
    };

    var address = _mapper.Map<AddressCreateDTO, Address>(createDto);

    var repoMock = new Mock<IAddressRepo>();
    repoMock
      .Setup(repo => repo.CreateOneAsync(It.IsAny<Address>()))
      .ReturnsAsync(address);

    var addressService = new AddressService(repoMock.Object, _mapper);

    // Act
    var result = await addressService.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Address>()), Times.Once);
  }

  [Fact]
  public async Task UpdateOneAsync_ValidAddress_ReturnsAddressReadDTO()
  {
    // Arrange
    var addressId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var updateDto = new AddressUpdateDTO
    {
      PostCode = "Updated PostCode",
      Street = "Updated St",
      City = "Updated City",
      Country = "Updated Country",
      UserId = userId
    };

    var address = new Address
    {
      Id = addressId,
      PostCode = "00100",
      Street = "123 Main St",
      City = "City",
      Country = "Country",
      UserId = userId
    };

    var repoMock = new Mock<IAddressRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(addressId))
        .ReturnsAsync(address);
    repoMock.Setup(repo => repo.UpdateOneAsync(It.IsAny<Address>()))
        .ReturnsAsync(address);

    var addressService = new AddressService(repoMock.Object, _mapper);

    // Act
    var result = await addressService.UpdateOneAsync(addressId, updateDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(updateDto.Street, result.Street);
    repoMock.Verify(repo => repo.GetByIdAsync(addressId), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<Address>()), Times.Once);
  }

  [Fact]
  public async Task GetByIdAsync_ExistingAddress_ReturnsAddressReadDTO()
  {
    // Arrange
    var addressId = Guid.NewGuid();

    var address = new Address
    {
      Id = addressId,
      PostCode = "00100",
      Street = "123 Main St",
      City = "City",
      Country = "Country",
      UserId = Guid.NewGuid()
    };

    var repoMock = new Mock<IAddressRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(addressId))
        .ReturnsAsync(address);

    var addressService = new AddressService(repoMock.Object, _mapper);

    // Act
    var result = await addressService.GetByIdAsync(addressId);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.GetByIdAsync(addressId), Times.Once);
  }

  [Fact]
  public async Task GetByIdAsync_NonExistingAddress_ThrowsCustomException()
  {
    // Arrange
    var addressId = Guid.NewGuid();
    var repoMock = new Mock<IAddressRepo>();
    repoMock
      .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync((Address?)null);

    var addressService = new AddressService(repoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await addressService.GetByIdAsync(addressId));

    // Assert
    repoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
  }

  [Fact]
  public async Task GetAllAsync_ReturnsGetAllResponse()
  {
    // Arrange
    var options = new GetAllOptions();

    var address1 = new Address
    {
      Id = Guid.NewGuid(),
      PostCode = "00100",
      Street = "123 Main St",
      City = "City",
      Country = "Country",
      UserId = Guid.NewGuid()
    };


    var address2 = new Address
    {
      Id = Guid.NewGuid(),
      PostCode = "00100",
      Street = "123 Main St",
      City = "City",
      Country = "Country",
      UserId = Guid.NewGuid()
    };

    var addresses = new List<Address> { address1, address2 };

    var repoMock = new Mock<IAddressRepo>();
    repoMock
      .Setup(repo => repo.GetAllAsync(options))
      .ReturnsAsync(addresses);

    repoMock
      .Setup(repo => repo.GetTotal(options))
      .ReturnsAsync(addresses.Count);

    var addressService = new AddressService(repoMock.Object, _mapper);

    // Act
    var result = await addressService.GetAllAsync(options);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(addresses.Count, result.Total);
    repoMock.Verify(repo => repo.GetAllAsync(options), Times.Once);
    repoMock.Verify(repo => repo.GetTotal(options), Times.Once);
  }
}