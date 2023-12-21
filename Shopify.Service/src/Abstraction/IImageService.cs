using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IImageService : IBaseService<Image, ImageReadDTO, ImageCreateDTO, ImageUpdateDTO>
{
}