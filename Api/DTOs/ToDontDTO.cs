using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public record CreateToDontDto
    {
        [Required]
        public required string Title { get; init; }
    }

    public record UpdateToDontDto
    {
        [Required]
        public required string Title { get; init; }

        public required bool IsActive { get; init; }
    }

    public record ToDontResponseDto(
        int Id,
        string Title,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        int ImageCount
    );
}
