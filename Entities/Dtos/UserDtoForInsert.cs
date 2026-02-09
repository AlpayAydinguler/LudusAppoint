namespace Entities.Dtos
{
    public record UserDtoForInsert : UserDto
    {
        public string? UserId { get; init; }
    }
}
