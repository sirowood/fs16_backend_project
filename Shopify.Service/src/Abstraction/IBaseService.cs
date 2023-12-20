using Shopify.Core.src.Shared;

namespace Shopify.Service.src.Abstraction;

public interface IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO>
{
  Task<IEnumerable<TReadDTO>> GetAllAsync(GetAllOptions options);
  Task<TReadDTO> GetByIdAsync(Guid id);
  Task<TReadDTO> CreateOneAsync(TCreateDTO createDTO);
  Task<TReadDTO> UpdateOneAsync(TUpdateDTO updateDTO);
  Task<bool> DeleteOneAsync(Guid id);
}