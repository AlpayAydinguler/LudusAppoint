namespace Entities.Dtos
{
    public record RoleDtoForUpdate : RoleDto
    {
        public List<string> Permissions { get; init; } = new List<string>();
    }
}
