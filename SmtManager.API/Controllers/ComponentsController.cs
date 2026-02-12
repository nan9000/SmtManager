using Microsoft.AspNetCore.Mvc;
using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;

namespace SmtManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComponentsController : ControllerBase
{
    private readonly IComponentService _componentService;

    public ComponentsController(IComponentService componentService)
    {
        _componentService = componentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var components = await _componentService.GetAllComponentsAsync();
        return Ok(components);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var component = await _componentService.GetComponentByIdAsync(id);
        if (component == null) return NotFound();
        return Ok(component);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateComponentDto createComponentDto)
    {
        var component = await _componentService.CreateComponentAsync(createComponentDto);
        return CreatedAtAction(nameof(GetById), new { id = component.Id }, component);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateComponentDto updateComponentDto)
    {
        try
        {
            await _componentService.UpdateComponentAsync(id, updateComponentDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _componentService.DeleteComponentAsync(id);
        return NoContent();
    }
}
