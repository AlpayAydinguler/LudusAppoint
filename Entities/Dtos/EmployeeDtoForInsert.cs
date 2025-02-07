namespace Entities.Dtos
{
    public record EmployeeDtoForInsert : EmployeeDto
    {
        public List<int> OfferedServiceIds { get; init; } = [];
    }
}
