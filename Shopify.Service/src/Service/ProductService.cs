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
  public ProductService(IProductRepo repo, ICategoryRepo categoryRepo, IMapper mapper) : base(repo, mapper)
  {
    _categoryRepo = categoryRepo;
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
    return await base.UpdateOneAsync(id, updateDTO);
  }
}