using Moq;
using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Core.src.Shared;
using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Test.src;

public class CategoryServiceTest
{
  private static IMapper GetMapper()
  {
    MapperConfiguration mappingConfig = new(m =>
    {
      m.AddProfile(new MapperProfile());
    });

    IMapper mapper = mappingConfig.CreateMapper();

    return mapper;
  }

  [Fact]
  public async void GetAllAsync_ShouldInvokeRepoMethod()
  {
    var repoMock = new Mock<ICategoryRepo>();
    var categoryService = new CategoryService(repoMock.Object, GetMapper());
    var options = new GetAllOptions();

    await categoryService.GetAllAsync(options);

    repoMock.Verify(repo => repo.GetAllAsync(options), Times.Once);
  }

  [Fact]
  public async Task CreateOneAsync_ValidCategory_ReturnsCategoryReadDTO()
  {
    // Arrange
    var createDto = new CategoryCreateDTO { Name = "NewCategory", Image = "http", };
    var repoMock = new Mock<ICategoryRepo>();

    repoMock.Setup(repo => repo.NameIsAvailable(It.IsAny<string>()))
      .ReturnsAsync(true);

    repoMock.Setup(repo => repo.CreateOneAsync(It.IsAny<Category>()))
      .ReturnsAsync(GetMapper().Map<CategoryCreateDTO, Category>(createDto));

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act
    var result = await service.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(createDto.Name, result.Name);
  }

  [Fact]
  public async Task CreateOneAsync_InValidCategory_ThrowsCustomException()
  {
    // Arrange
    var createDto = new CategoryCreateDTO { Name = "NewCategory", Image = "http" };
    var repoMock = new Mock<ICategoryRepo>();

    repoMock.Setup(repo => repo.NameIsAvailable(It.IsAny<string>()))
        .ReturnsAsync(false);

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<CustomException>(() => service.CreateOneAsync(createDto));

    // Assert
    Assert.Equal("Name is not available", exception.Message);
  }

  [Fact]
  public async Task GetAllAsync_ReturnsGetAllResponse()
  {
    // Arrange
    var repoMock = new Mock<ICategoryRepo>();
    var options = new GetAllOptions();

    var entities = new List<Category>
      {
        new() { Id = Guid.NewGuid(), Name = "Category1", Image = "http" },
        new() { Id = Guid.NewGuid(), Name = "Category2", Image = "http2" },
      };

    repoMock.Setup(repo => repo.GetAllAsync(options))
        .ReturnsAsync(entities);

    repoMock.Setup(repo => repo.GetTotal(options))
        .ReturnsAsync(entities.Count);

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act
    var result = await service.GetAllAsync(options);

    // Assert
    Assert.NotNull(result);
    Assert.IsType<GetAllResponse<CategoryReadDTO>>(result);
    Assert.Equal(entities.Count, result.Total);

    var items = Assert.IsAssignableFrom<IEnumerable<CategoryReadDTO>>(result.Items);
    Assert.Equal(entities.Count, items.Count());
  }

  [Fact]
  public async Task UpdateOneAsync_CategoryExists_ReturnsUpdatedCategoryReadDTO()
  {
    // Arrange
    var categoryId = Guid.NewGuid();
    var originalEntity = new Category
    {
      Id = categoryId,
      Name = "ExistingCategory",
      Image = "http"
    };

    var updateDto = new CategoryUpdateDTO
    {
      Name = "UpdatedCategory",
      Image = "http"
    };

    var updatedDto = new Category
    {
      Id = categoryId,
      Name = "UpdatedCategory",
      Image = "http"
    };

    var repoMock = new Mock<ICategoryRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(categoryId))
      .ReturnsAsync(originalEntity);
    repoMock.Setup(repo => repo.NameIsAvailable(It.IsAny<string>()))
      .ReturnsAsync(true);
    repoMock.Setup(repo => repo.UpdateOneAsync(It.IsAny<Category>()))
      .ReturnsAsync(updatedDto);

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act
    var result = await service.UpdateOneAsync(categoryId, updateDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(updateDto.Name, result.Name);
  }

  [Fact]
  public async Task DeleteOneAsync_EntityExists_ReturnsTrue()
  {
    // Arrange
    var entityId = Guid.NewGuid();

    var entityToDelete = new Category
    {
      Id = entityId,
      Name = "New category",
      Image = "http"
    };

    var repoMock = new Mock<ICategoryRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(entityId))
        .ReturnsAsync(entityToDelete);

    repoMock.Setup(repo => repo.DeleteOneAsync(It.IsAny<Category>()))
        .ReturnsAsync(true);

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act
    var result = await service.DeleteOneAsync(entityId);

    // Assert
    Assert.True(result);
    repoMock.Verify(repo => repo.DeleteOneAsync(entityToDelete), Times.Once);
  }

  [Fact]
  public async Task DeleteOneAsync_EntityNotFound_ThrowsCustomException()
  {
    // Arrange
    var entityId = Guid.NewGuid();

    var repoMock = new Mock<ICategoryRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(entityId))
      .ReturnsAsync((Category)null!);

    var service = new CategoryService(repoMock.Object, GetMapper());

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await service.DeleteOneAsync(entityId));
    repoMock.Verify(repo => repo.DeleteOneAsync(It.IsAny<Category>()), Times.Never);
  }
}