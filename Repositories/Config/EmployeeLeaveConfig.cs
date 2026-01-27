using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class EmployeeLeaveConfig : IEntityTypeConfiguration<EmployeeLeave>
    {
        public void Configure(EntityTypeBuilder<EmployeeLeave> builder)
        {
            
            var employeeLeave1 = new EmployeeLeave { EmployeeLeaveId = 1, EmployeeId = 1, LeaveStartDateTime = System.DateTime.Today.AddDays(1) + new TimeSpan(8, 0, 0), LeaveEndDateTime = System.DateTime.Today.AddDays(1) + new TimeSpan(18, 0, 0), Reason = "Sick", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            var employeeLeave2 = new EmployeeLeave { EmployeeLeaveId = 2, EmployeeId = 2, LeaveStartDateTime = System.DateTime.Today.AddDays(2) + new TimeSpan(8, 0, 0), LeaveEndDateTime = System.DateTime.Today.AddDays(2) + new TimeSpan(18, 0, 0), Reason = "Vacation", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            var employeeLeave3 = new EmployeeLeave { EmployeeLeaveId = 3, EmployeeId = 3, LeaveStartDateTime = System.DateTime.Today.AddDays(3) + new TimeSpan(8, 0, 0), LeaveEndDateTime = System.DateTime.Today.AddDays(3) + new TimeSpan(18, 0, 0), Reason = "Personal", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            var employeeLeaves = new List<EmployeeLeave>();
            employeeLeaves.Add(employeeLeave1);
            employeeLeaves.Add(employeeLeave2);
            employeeLeaves.Add(employeeLeave3);
            builder.HasData(employeeLeaves);
            builder.HasOne(e => e.Tenant)
                   .WithMany()
                   .HasForeignKey(e => e.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
