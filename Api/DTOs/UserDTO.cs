using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record CreateUserDto(
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    string UserName,

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    string Password
);

public record UserResponseDto(int Id, string UserName, int ToDontCount);

public record LoginDto(
    [Required(ErrorMessage = "Username is required")]
    string UserName,

    [Required(ErrorMessage = "Password is required")]
    string Password
);

public record LoginResponseDto(string Token, int UserId, string UserName);
