using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Services;

public class ComponentService : IComponentService
{
    private readonly IGenericRepository<Component> _repository;

    public ComponentService(IGenericRepository<Component> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ComponentDto>> GetAllComponentsAsync()
    {
        var components = await _repository.GetAllAsync();
        return components.Select(c => new ComponentDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Quantity = c.Quantity
        });
    }

    public async Task<ComponentDto?> GetComponentByIdAsync(int id)
    {
        var component = await _repository.GetByIdAsync(id);
        if (component == null) return null;

        return new ComponentDto
        {
            Id = component.Id,
            Name = component.Name,
            Description = component.Description,
            Quantity = component.Quantity
        };
    }

    public async Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto)
    {
        var component = new Component
        {
            Name = createComponentDto.Name,
            Description = createComponentDto.Description,
            Quantity = createComponentDto.Quantity
        };

        await _repository.AddAsync(component);

        return new ComponentDto
        {
            Id = component.Id,
            Name = component.Name,
            Description = component.Description,
            Quantity = component.Quantity
        };
    }

    public async Task UpdateComponentAsync(int id, UpdateComponentDto updateComponentDto)
    {
        var component = await _repository.GetByIdAsync(id);
        if (component == null) throw new KeyNotFoundException($"Component with ID {id} not found");

        component.Name = updateComponentDto.Name;
        component.Description = updateComponentDto.Description;
        component.Quantity = updateComponentDto.Quantity;

        await _repository.UpdateAsync(component);
    }

    public async Task DeleteComponentAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
