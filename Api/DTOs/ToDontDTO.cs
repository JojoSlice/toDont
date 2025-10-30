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

    public record ToDontResponseDto
    {
        public required int Id { get; init; }

        [Required]
        public required string Title { get; init; }

        public required bool IsActive { get; init; }
    }
}
