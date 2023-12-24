using Shopify.Core.src.Shared;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO>
{
  Task<GetAllResponse<TReadDTO>> GetAllAsync(GetAllOptions options);
  Task<TReadDTO> GetByIdAsync(Guid id);
  Task<TReadDTO> CreateOneAsync(TCreateDTO createDTO);
  Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updateDTO);
  Task<bool> DeleteOneAsync(Guid id);
}