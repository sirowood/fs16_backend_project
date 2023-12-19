using AutoMapper;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;

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

  public virtual TReadDTO CreateOne(TCreateDTO createDTO)
  {
    var newObject = _mapper.Map<TCreateDTO, T>(createDTO);
    var createdObject = _repo.CreateOne(newObject);

    return _mapper.Map<T, TReadDTO>(createdObject);
  }

  public virtual bool DeleteOne(Guid id)
  {
    throw new NotImplementedException();
  }

  public virtual IEnumerable<TReadDTO> GetAll(GetAllOptions options)
  {
    var objects = _repo.GetAll(options);

    return _mapper.Map<IEnumerable<T>, IEnumerable<TReadDTO>>(objects);
  }

  public virtual TReadDTO GetById(Guid id)
  {
    throw new NotImplementedException();
  }

  public virtual TReadDTO UpdateOne(TUpdateDTO updateDTO)
  {
    throw new NotImplementedException();
  }
}