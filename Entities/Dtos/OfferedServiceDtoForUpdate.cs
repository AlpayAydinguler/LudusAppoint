namespace Entities.Dtos
{
    public record OfferedServiceDtoForUpdate : OfferedServiceDto
    {
        public List<int> AgeGroupIds { get; init; } = new();
        public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();
    }
}
