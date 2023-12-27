using AutoMapper;

using Moq;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Service.Tests;

public class ImageServiceTests
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();

  [Fact]
  public async Task CreateOneAsync_ValidImage_ReturnsImageReadDTO()
  {
    // Arrange
    var createDto = new ImageCreateDTO
    {
      ProductId = Guid.NewGuid(),
      URL = "http://example.com/image.jpg"
    };

    var image = _mapper.Map<ImageCreateDTO, Image>(createDto);

    var repoMock = new Mock<IImageRepo>();
    repoMock.Setup(repo => repo.CreateOneAsync(It.IsAny<Image>()))
      .ReturnsAsync(image);

    var product = new Product
    {
      Id = new Guid(),
      Title = "title",
      Description = "description",
      Price = 19.9m,
      CategoryId = new Guid()
    };

    var productRepoMock = new Mock<IProductRepo>();
    productRepoMock.Setup(productRepo => productRepo.GetByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync(product);

    var imageService = new ImageService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act
    var result = await imageService.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Image>()), Times.Once);
    productRepoMock.Verify(productRepo => productRepo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
  }

  [Fact]
  public async Task CreateOneAsync_InvalidProduct_ThrowsCustomException()
  {
    // Arrange
    var createDto = new ImageCreateDTO
    {
      ProductId = Guid.NewGuid(),
      URL = "http://example.com/image.jpg"
    };

    var repoMock = new Mock<IImageRepo>();
    var productRepoMock = new Mock<IProductRepo>();
    productRepoMock.Setup(productRepo => productRepo.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Product?)null);

    var imageService = new ImageService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await imageService.CreateOneAsync(createDto));

    // Assert
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Image>()), Times.Never);
    productRepoMock.Verify(productRepo => productRepo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
  }

  [Fact]
  public async Task UpdateOneAsync_ValidImage_ReturnsImageReadDTO()
  {
    // Arrange
    var imageId = Guid.NewGuid();
    var updateDto = new ImageUpdateDTO
    {
      ProductId = Guid.NewGuid(),
      URL = "http://example.com/updated-image.jpg"
    };

    var product = new Product
    {
      Id = updateDto.ProductId,
      Title = "title",
      Description = "description",
      Price = 19.9m,
      CategoryId = new Guid()
    };

    var image = _mapper.Map<ImageUpdateDTO, Image>(updateDto);

    var repoMock = new Mock<IImageRepo>();
    repoMock.Setup(repo => repo.UpdateOneAsync(It.IsAny<Image>()))
      .ReturnsAsync(image);
    repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync(image);

    var productRepoMock = new Mock<IProductRepo>();
    productRepoMock.Setup(productRepo => productRepo.GetByIdAsync(updateDto.ProductId))
      .ReturnsAsync(product);

    var imageService = new ImageService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act
    var result = await imageService.UpdateOneAsync(imageId, updateDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<Image>()), Times.Once);
    productRepoMock.Verify(productRepo => productRepo.GetByIdAsync(updateDto.ProductId), Times.Once);
  }

  [Fact]
  public async Task UpdateOneAsync_InvalidProduct_ThrowsCustomException()
  {
    // Arrange
    var imageId = Guid.NewGuid();
    var updateDto = new ImageUpdateDTO { ProductId = Guid.NewGuid(), URL = "http://example.com/updated-image.jpg" };

    var repoMock = new Mock<IImageRepo>();
    var productRepoMock = new Mock<IProductRepo>();
    productRepoMock
      .Setup(productRepo => productRepo.GetByIdAsync(updateDto.ProductId))
      .ReturnsAsync((Product?)null);

    var imageService = new ImageService(repoMock.Object, productRepoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await imageService.UpdateOneAsync(imageId, updateDto));

    // Assert
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<Image>()), Times.Never);
    productRepoMock.Verify(productRepo => productRepo.GetByIdAsync(updateDto.ProductId), Times.Once);
  }
}
