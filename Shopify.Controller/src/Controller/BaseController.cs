using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;

namespace Shopify.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class BaseController<T, TReadDTO, TCreateDTO, TUpdateDTO> : ControllerBase where T : BaseEntity
{

  protected readonly IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> _service;

  public BaseController(IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> service)
  {
    _service = service;
  }

  [HttpPost]
  public virtual ActionResult<TReadDTO> CreateOne([FromBody] TCreateDTO createDTO)
  {
    var createdObject = _service.CreateOne(createDTO);

    return CreatedAtAction(nameof(CreateOne), createdObject);
  }

  [HttpDelete("{id:guid}")]
  public virtual ActionResult<bool> DeleteOne([FromRoute] Guid id)
  {
    return Ok(_service.DeleteOne(id));
  }

  [HttpGet()]
  public virtual ActionResult<IEnumerable<TReadDTO>> GetAll([FromQuery] GetAllOptions options)
  {
    return Ok(_service.GetAll(options));
  }

  [HttpGet("{id:guid}")]
  public virtual ActionResult<TReadDTO> GetById([FromRoute] Guid id)
  {
    return Ok(_service.GetById(id));
  }

  [HttpPatch("{id:guid}")]
  public virtual ActionResult<TReadDTO> UpdateOne([FromRoute] Guid id, [FromBody] TUpdateDTO updateDTO)
  {
    return Ok(_service.UpdateOne(updateDTO));
  }
}