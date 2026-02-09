namespace Entities.Dtos
{
    public record RoleDtoForInsert : RoleDto
    {
        public string? RoleId { get; init; }
        public List<string> Permissions { get; init; } = new List<string>();
    }
}
