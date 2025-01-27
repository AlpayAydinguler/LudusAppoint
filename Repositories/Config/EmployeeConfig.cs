using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasMany(os => os.OfferedServices)
                   .WithMany(e => e.Employees)
                   .UsingEntity<Dictionary<string, object>>("EmployeeOfferedServices",
                                                            b => b.HasOne<OfferedService>().WithMany().HasForeignKey("OfferedServicesId"),
                                                            b => b.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                                                            b =>
                                                            {
                                                                b.Property<int>("EmployeeId");
                                                                b.Property<int>("OfferedServicesId");
                                                                b.HasKey("EmployeeId", "OfferedServicesId");
                                                            });

            var employee1 = new Employee { EmployeeId = 1, IdentityUserId = "1", EmployeeName = "Aydın", EmployeeSurname = "Sevim", CanTakeClients = true, BranchId = 1, DayOff = DayOfWeek.Sunday, StartOfWorkingHours = new TimeSpan(10, 0, 0), EndOfWorkingHours = new TimeSpan(19, 0, 0) };
            var employee2 = new Employee { EmployeeId = 2, IdentityUserId = "2", EmployeeName = "Alpay", EmployeeSurname = "Aydıngüler", CanTakeClients = true, BranchId = 1, DayOff = DayOfWeek.Sunday, StartOfWorkingHours = new TimeSpan(10, 0, 0), EndOfWorkingHours = new TimeSpan(19, 0, 0) };
            var employee3 = new Employee { EmployeeId = 3, IdentityUserId = "3", EmployeeName = "Deniz", EmployeeSurname = "Dağ", CanTakeClients = false, BranchId = 1, DayOff = DayOfWeek.Sunday, StartOfWorkingHours = new TimeSpan(10, 0, 0), EndOfWorkingHours = new TimeSpan(19, 0, 0) };
            var employees = new List<Employee>();
            employees.Add(employee1);
            employees.Add(employee2);
            employees.Add(employee3);

            builder.HasData(employees);
        }
    }
}
