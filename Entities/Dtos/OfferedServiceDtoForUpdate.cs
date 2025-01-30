namespace Entities.Dtos
{
    public record OfferedServiceDtoForUpdate : OfferedServiceDto
    {
        public List<int> AgeGroupIds { get; init; } = new();
    }
}
