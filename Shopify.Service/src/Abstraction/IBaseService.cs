using Shopify.Core.src.Shared;

namespace Shopify.Service.src.Abstraction;

public interface IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO>
{
  IEnumerable<TReadDTO> GetAll(GetAllOptions options);
  TReadDTO GetById(Guid id);
  TReadDTO CreateOne(TCreateDTO createDTO);
  TReadDTO UpdateOne(TUpdateDTO updateDTO);
  bool DeleteOne(Guid id);
}