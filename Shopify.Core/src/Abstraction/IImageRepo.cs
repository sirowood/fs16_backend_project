using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface IImageRepo : IBaseRepo<Image>
{
  Task RemoveImagesByProductId(Guid id);
}