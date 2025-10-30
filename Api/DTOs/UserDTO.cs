namespace Api.Dtos;

public record CreateUserDto(string UserName, string Password);

public record UserResponseDto(int Id, string UserName, int ToDontCount);

public record LoginDto(string UserName, string Password);

public record LoginResponseDto(string Token, int UserId, string UserName);
