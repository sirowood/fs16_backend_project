using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

  [HttpPost()]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public virtual async Task<ActionResult<TReadDTO>> CreateOneAsync([FromBody] TCreateDTO createDTO)
  {
    var createdObject = await _service.CreateOneAsync(createDTO);
    return CreatedAtAction(nameof(CreateOneAsync), createdObject);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{id:guid}")]
  public virtual async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    var result = await _service.DeleteOneAsync(id);

    return Ok(result);
  }

  [HttpGet()]
  public virtual async Task<ActionResult<IEnumerable<TReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    var result = await _service.GetAllAsync(options);

    return Ok(result);
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<ActionResult<TReadDTO>> GetByIdAsync([FromRoute] Guid id)
  {
    var result = await _service.GetByIdAsync(id);

    return Ok(result);
  }

  [HttpPatch("{id:guid}")]
  public virtual async Task<ActionResult<TReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] TUpdateDTO updateDTO)
  {
    var result = await _service.UpdateOneAsync(updateDTO);

    return Ok(result);
  }
}