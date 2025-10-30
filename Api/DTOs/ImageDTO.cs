namespace Api.Dtos;

public record ImageResponseDto(
    int Id,
    string FileName
);

public record CreateImageDto(
    string FileName,
    int ToDontId
);

public record UpdateImageDto(
    string FileName
);
