using Api.Dtos;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDontController(IToDontService toDontService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDontResponseDto>>> GetAllToDoNts()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var toDonts = await toDontService.GetAllByUserIdAsync(userId);

            var response = toDonts.Select(t => new ToDontResponseDto(
                t.Id,
                t.Title,
                t.IsActive,
                t.CreatedAt,
                t.UpdatedAt,
                t.Images.Count
            ));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDontResponseDto>> GetToDontById(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var toDont = await toDontService.GetByIdAsync(id, userId);

            if (toDont == null)
                return NotFound();

            return new ToDontResponseDto(
                toDont.Id,
                toDont.Title,
                toDont.IsActive,
                toDont.CreatedAt,
                toDont.UpdatedAt,
                toDont.Images.Count
            );
        }

        [HttpPost]
        public async Task<ActionResult<ToDontResponseDto>> CreateToDont([FromBody] CreateToDontDto request)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            var toDont = new ToDont
            {
                Title = request.Title,
                UserId = userId,
                IsActive = true
            };

            var created = await toDontService.CreateAsync(toDont);

            var response = new ToDontResponseDto(
                created.Id,
                created.Title,
                created.IsActive,
                created.CreatedAt,
                created.UpdatedAt,
                created.Images.Count
            );

            return CreatedAtAction(
                nameof(GetToDontById),
                new { id = created.Id },
                response
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDontResponseDto>> UpdateToDont(int id, [FromBody] UpdateToDontDto request)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var toDont = new ToDont { Title = request.Title, IsActive = request.IsActive };

            var updated = await toDontService.UpdateAsync(id, userId, toDont);

            if (updated == null)
                return NotFound();

            return new ToDontResponseDto(
                updated.Id,
                updated.Title,
                updated.IsActive,
                updated.CreatedAt,
                updated.UpdatedAt,
                updated.Images.Count
            );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDont(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var success = await toDontService.DeleteAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult> ToggleToDont(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var success = await toDontService.ToggleActiveAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
