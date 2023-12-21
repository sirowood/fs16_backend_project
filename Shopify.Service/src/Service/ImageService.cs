using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class ImageService : BaseService<Image, ImageReadDTO, ImageCreateDTO, ImageUpdateDTO>, IImageService
{
  private readonly IProductRepo _productRepo;
  public ImageService(IImageRepo repo, IProductRepo productRepo, IMapper mapper) : base(repo, mapper)
  {
    _productRepo = productRepo;
  }

  private async Task VerifyProduct(Guid? productId)
  {
    if (productId.HasValue)
    {
      _ = await _productRepo.GetByIdAsync(productId.Value)
          ?? throw CustomException.NotFound("Invalid product.");
    }
    else
    {
      throw CustomException.NotFound("Invalid product.");
    }
  }

  public override async Task<ImageReadDTO> CreateOneAsync(ImageCreateDTO createDTO)
  {
    await VerifyProduct(createDTO.ProductId);

    return await base.CreateOneAsync(createDTO);
  }

  public override async Task<ImageReadDTO> UpdateOneAsync(Guid id, ImageUpdateDTO updateDTO)
  {
    await VerifyProduct(updateDTO.ProductId);

    return await base.UpdateOneAsync(id, updateDTO);
  }
}