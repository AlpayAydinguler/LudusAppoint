namespace Entities.Dtos
{
    public record EmployeeDtoForUpdate : EmployeeDto
    {
        public List<int> OfferedServiceIds { get; init; } = [];
    }
}
