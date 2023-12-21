using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IProductService : IBaseService<Product, ProductReadDTO, ProductCreateDTO, ProductUpdateDTO>
{
}