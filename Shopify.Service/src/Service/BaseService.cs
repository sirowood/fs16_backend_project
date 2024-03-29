using AutoMapper;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class BaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> : IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO>
where T : BaseEntity
{
  protected readonly IBaseRepo<T> _repo;
  protected readonly IMapper _mapper;

  public BaseService(IBaseRepo<T> repo, IMapper mapper)
  {
    _repo = repo;
    _mapper = mapper;
  }

  public virtual async Task<TReadDTO> CreateOneAsync(TCreateDTO createDTO)
  {
    var newObject = _mapper.Map<TCreateDTO, T>(createDTO);
    var createdObject = await _repo.CreateOneAsync(newObject);

    return _mapper.Map<T, TReadDTO>(createdObject);
  }

  public virtual async Task<bool> DeleteOneAsync(Guid id)
  {
    var deleteObject = await _repo.GetByIdAsync(id)
      ?? throw CustomException.NotFound();

    var result = await _repo.DeleteOneAsync(deleteObject);

    return result;
  }

  public virtual async Task<GetAllResponse<TReadDTO>> GetAllAsync(GetAllOptions options)
  {
    var entities = await _repo.GetAllAsync(options);

    var readEntities = _mapper.Map<IEnumerable<T>, IEnumerable<TReadDTO>>(entities);

    var total = await _repo.GetTotal(options);

    var pages = (int)Math.Ceiling((double)total / options.Limit);

    var result = new GetAllResponse<TReadDTO> { Items = readEntities, Total = total, Pages = pages };

    return result;
  }

  public virtual async Task<TReadDTO> GetByIdAsync(Guid id)
  {
    var result = await _repo.GetByIdAsync(id)
      ?? throw CustomException.NotFound();

    return _mapper.Map<T, TReadDTO>(result);
  }

  public virtual async Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updateDTO)
  {
    var originalEntity = await _repo.GetByIdAsync(id)
      ?? throw CustomException.NotFound();

    var updatedEntity = _mapper.Map(updateDTO, originalEntity);
    updatedEntity.UpdatedAt = DateTime.UtcNow;

    var result = await _repo.UpdateOneAsync(updatedEntity);

    return _mapper.Map<T, TReadDTO>(result);
  }
}