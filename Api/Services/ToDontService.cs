using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ToDontService(AppDbContext context) : IToDontService
{
    public async Task<IEnumerable<ToDont>> GetAllByUserIdAsync(int userId)
    {
        return await context
            .ToDonts.Where(t => t.UserId == userId)
            .Include(t => t.Images)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<ToDont?> GetByIdAsync(int id, int userId)
    {
        return await context
            .ToDonts.Include(t => t.Images)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<ToDont> CreateAsync(ToDont toDont)
    {
        toDont.CreatedAt = DateTime.UtcNow;
        toDont.UpdatedAt = DateTime.UtcNow;

        context.ToDonts.Add(toDont);
        await context.SaveChangesAsync();
        return toDont;
    }

    public async Task<ToDont?> UpdateAsync(int id, int userId, ToDont toDont)
    {
        var existing = await GetByIdAsync(id, userId);
        if (existing == null)
            return null;

        existing.Title = toDont.Title;
        existing.IsActive = toDont.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var toDont = await GetByIdAsync(id, userId);
        if (toDont == null)
            return false;

        context.ToDonts.Remove(toDont);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(int id, int userId)
    {
        var toDont = await GetByIdAsync(id, userId);
        if (toDont == null)
            return false;

        toDont.IsActive = !toDont.IsActive;
        toDont.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }
}
