using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class CategoryService : BaseService<Category, CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>, ICategoryService
{
  private new readonly ICategoryRepo _repo;
  public CategoryService(ICategoryRepo repo, IMapper mapper) : base(repo, mapper)
  {
    _repo = repo;
  }

  private async Task CheckNameAvailability(string name)
  {
    var nameIsAvailable = await _repo.NameIsAvailable(name);

    if (!nameIsAvailable)
    {
      throw CustomException.NotAvailable("Name is not available");
    }
  }

  public override async Task<CategoryReadDTO> CreateOneAsync(CategoryCreateDTO createDTO)
  {
    await CheckNameAvailability(createDTO.Name);

    return await base.CreateOneAsync(createDTO);
  }

  public override async Task<CategoryReadDTO> UpdateOneAsync(Guid id, CategoryUpdateDTO updateDTO)
  {
    var originalEntity = await _repo.GetByIdAsync(id)
      ?? throw CustomException.NotFound();

    if (updateDTO.Name != originalEntity.Name)
    {
      await CheckNameAvailability(updateDTO.Name);
    }

    var result = await base.UpdateOneAsync(id, updateDTO);

    return result;
  }
}