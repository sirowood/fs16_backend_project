using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class ProductService : BaseService<Product, ProductReadDTO, ProductCreateDTO, ProductUpdateDTO>, IProductService
{
  private readonly ICategoryRepo _categoryRepo;
  private readonly IImageRepo _imageRepo;

  public ProductService(IProductRepo repo, ICategoryRepo categoryRepo, IImageRepo imageRepo, IMapper mapper) : base(repo, mapper)
  {
    _categoryRepo = categoryRepo;
    _imageRepo = imageRepo;
  }

  private async Task VerifyCategory(Guid categoryId)
  {
    _ = await _categoryRepo.GetByIdAsync(categoryId)
      ?? throw CustomException.NotFound("Invalid category.");
  }

  public override async Task<ProductReadDTO> CreateOneAsync(ProductCreateDTO createDTO)
  {
    await VerifyCategory(createDTO.CategoryId);
    return await base.CreateOneAsync(createDTO);
  }

  public override async Task<ProductReadDTO> UpdateOneAsync(Guid id, ProductUpdateDTO updateDTO)
  {
    await VerifyCategory(updateDTO.CategoryId);

    await _imageRepo.RemoveImagesByProductId(id);

    return await base.UpdateOneAsync(id, updateDTO);
  }
}