namespace Dtos
{
    public class CreateToDontDto
    {
        public string Title { get; set; }
    }

    public class UpdateToDontDto
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }

    public class ToDontResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
