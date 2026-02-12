using SmtManager.Application.DTOs;

namespace SmtManager.Application.Interfaces;

public interface IComponentService
{
    Task<IEnumerable<ComponentDto>> GetAllComponentsAsync();
    Task<ComponentDto?> GetComponentByIdAsync(int id);
    Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto);
    Task UpdateComponentAsync(int id, UpdateComponentDto updateComponentDto);
    Task DeleteComponentAsync(int id);
}
