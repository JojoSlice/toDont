using Api.Dtos;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDontController(IToDontService toDontService) : ControllerBase
    {
        // TODO auth - userId should come from authenticated user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDontResponseDto>>> GetAllToDoNts([FromQuery] int userId)
        {
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

        // TODO auth - userId should come from authenticated user
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDontResponseDto>> GetToDontById(int id, [FromQuery] int userId)
        {
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

        // TODO auth - userId should come from authenticated user
        [HttpPost]
        public async Task<ActionResult<ToDontResponseDto>> CreateToDont(
            [FromBody] CreateToDontDto request,
            [FromQuery] int userId
        )
        {
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
                new { id = created.Id, userId },
                response
            );
        }

        // TODO auth - userId should come from authenticated user
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDontResponseDto>> UpdateToDont(
            int id,
            [FromBody] UpdateToDontDto request,
            [FromQuery] int userId
        )
        {
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

        // TODO auth - userId should come from authenticated user
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDont(int id, [FromQuery] int userId)
        {
            var success = await toDontService.DeleteAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // TODO auth - userId should come from authenticated user
        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult> ToggleToDont(int id, [FromQuery] int userId)
        {
            var success = await toDontService.ToggleActiveAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
