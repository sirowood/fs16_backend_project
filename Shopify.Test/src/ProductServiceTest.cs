using AutoMapper;
using Moq;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Test.src;

public class ProductServiceTest
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();
  [Fact]
  public async Task CreateOneAsync_ValidProduct_ReturnsProductReadDTO()
  {
    // Arrange
    var categoryId = Guid.NewGuid();
    var category = new Category
    {
      Id = categoryId,
      Name = "name",
      Image = "image"
    };

    var createDto = new ProductCreateDTO
    {
      Title = "Product",
      Description = "description",
      Price = 19.9m,
      CategoryId = categoryId,
      Images = new List<ImageCreateDTO>()
    };

    var product = _mapper.Map<ProductCreateDTO, Product>(createDto);

    var categoryRepoMock = new Mock<ICategoryRepo>();
    categoryRepoMock
      .Setup(repo => repo.GetByIdAsync(createDto.CategoryId))
      .ReturnsAsync(category);

    var imageRepoMock = new Mock<IImageRepo>();

    var repoMock = new Mock<IProductRepo>();
    repoMock
    .Setup(repo => repo.CreateOneAsync(It.IsAny<Product>()))
      .ReturnsAsync(product);

    var productService = new ProductService(repoMock.Object, categoryRepoMock.Object, imageRepoMock.Object, _mapper);

    // Act
    var result = await productService.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    categoryRepoMock.Verify(repo => repo.GetByIdAsync(createDto.CategoryId), Times.Once);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Product>()), Times.Once);
  }

  [Fact]
  public async Task CreateOneAsync_InvalidCategory_ThrowsCustomException()
  {
    // Arrange

    var createDto = new ProductCreateDTO
    {
      Title = "Product",
      Description = "description",
      Price = 19.9m,
      CategoryId = Guid.NewGuid(),
      Images = new List<ImageCreateDTO>()
    };

    var categoryRepoMock = new Mock<ICategoryRepo>();
    categoryRepoMock
      .Setup(repo => repo.GetByIdAsync(createDto.CategoryId))
      .ReturnsAsync((Category?)null);

    var imageRepoMock = new Mock<IImageRepo>();

    var repoMock = new Mock<IProductRepo>();

    var productService = new ProductService(repoMock.Object, categoryRepoMock.Object, imageRepoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await productService.CreateOneAsync(createDto));

    // Assert
    categoryRepoMock.Verify(repo => repo.GetByIdAsync(createDto.CategoryId), Times.Once);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<Product>()), Times.Never);
  }

  [Fact]
  public async Task UpdateOneAsync_ValidProduct_ReturnsProductReadDTO()
  {
    // Arrange
    var productId = Guid.NewGuid();
    var categoryId = Guid.NewGuid();
    var category = new Category
    {
      Id = categoryId,
      Name = "name",
      Image = "image"
    };

    var updateDto = new ProductUpdateDTO
    {
      Title = "UpdatedProduct",
      Description = "description",
      Price = 19.9m,
      CategoryId = categoryId,
      Images = new List<ImageCreateDTO>(),
    };

    var product = _mapper.Map<ProductUpdateDTO, Product>(updateDto);

    var categoryRepoMock = new Mock<ICategoryRepo>();
    categoryRepoMock
      .Setup(repo => repo.GetByIdAsync(updateDto.CategoryId))
      .ReturnsAsync(category);

    var imageRepoMock = new Mock<IImageRepo>();

    var repoMock = new Mock<IProductRepo>();
    repoMock
      .Setup(repo => repo.UpdateOneAsync(It.IsAny<Product>()))
      .ReturnsAsync(product);
    repoMock
      .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync(product);

    var productService = new ProductService(repoMock.Object, categoryRepoMock.Object, imageRepoMock.Object, _mapper);

    // Act
    var result = await productService.UpdateOneAsync(productId, updateDto);

    // Assert
    Assert.NotNull(result);
    categoryRepoMock.Verify(repo => repo.GetByIdAsync(updateDto.CategoryId), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<Product>()), Times.Once);
    Assert.Equal(product.Title, result.Title);
  }

  [Fact]
  public async Task UpdateOneAsync_InvalidCategory_ThrowsCustomException()
  {
    // Arrange
    var productId = Guid.NewGuid();

    var updateDto = new ProductUpdateDTO
    {
      Title = "UpdatedProduct",
      Description = "description",
      Price = 19.9m,
      CategoryId = Guid.NewGuid(),
      Images = new List<ImageCreateDTO>(),
    };

    var categoryRepoMock = new Mock<ICategoryRepo>();
    categoryRepoMock.Setup(repo => repo.GetByIdAsync(updateDto.CategoryId))
        .ReturnsAsync((Category?)null);

    var imageRepoMock = new Mock<IImageRepo>();

    var repoMock = new Mock<IProductRepo>();

    var productService = new ProductService(repoMock.Object, categoryRepoMock.Object, imageRepoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await productService.UpdateOneAsync(productId, updateDto));

    // Assert
    categoryRepoMock.Verify(repo => repo.GetByIdAsync(updateDto.CategoryId), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<Product>()), Times.Never);
  }
}