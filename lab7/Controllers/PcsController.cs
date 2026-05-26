using lab7.DTOs;
using lab7.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PcsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PcGetDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PcGetDto>>> GetAll()
    {
        var pcs = await _pcService.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    [ProducesResponseType(typeof(PcWithComponentsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PcWithComponentsDto>> GetWithComponents(int id)
    {
        var pc = await _pcService.GetWithComponentsAsync(id);

        if (pc is null)
        {
            return NotFound();
        }

        return Ok(pc);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PcGetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PcGetDto>> Create(PcCreateDto dto)
    {
        var createdPc = await _pcService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetWithComponents), new { id = createdPc.Id }, createdPc);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PcGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PcGetDto>> Update(int id, PcUpdateDto dto)
    {
        var updatedPc = await _pcService.UpdateAsync(id, dto);

        if (updatedPc is null)
        {
            return NotFound();
        }

        return Ok(updatedPc);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pcService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
