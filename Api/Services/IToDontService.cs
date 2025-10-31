using Api.Models;

namespace Api.Services;

public interface IToDontService
{
    Task<IEnumerable<ToDont>> GetAllByUserIdAsync(int userId);
    Task<ToDont?> GetByIdAsync(int id, int userId);
    Task<ToDont> CreateAsync(ToDont toDont);
    Task<ToDont?> UpdateAsync(int id, int userId, ToDont toDont);
    Task<bool> DeleteAsync(int id, int userId);
    Task<bool> ToggleActiveAsync(int id, int userId);
}
