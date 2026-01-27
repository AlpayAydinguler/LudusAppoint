using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class CustomerAppointmentConfig : IEntityTypeConfiguration<CustomerAppointment>
    {
        public void Configure(EntityTypeBuilder<CustomerAppointment> builder)
        {
            builder.HasMany(c => c.OfferedServices)
                   .WithMany(o => o.CustomerAppointments)
                   .UsingEntity<Dictionary<string, object>>("CustomerAppointmentOfferedServices",
                                                            b => b.HasOne<OfferedService>().WithMany().HasForeignKey("OfferedServicesId").OnDelete(DeleteBehavior.Restrict),
                                                            b => b.HasOne<CustomerAppointment>().WithMany().HasForeignKey("CustomerAppointmentId").OnDelete(DeleteBehavior.Restrict),
                                                            b =>
                                                            {
                                                                b.Property<int>("CustomerAppointmentId");
                                                                b.Property<int>("OfferedServicesId");
                                                                b.HasKey("CustomerAppointmentId", "OfferedServicesId");
                                                            });

            // Configure the relationship with AgeGroup
            builder.HasOne(ca => ca.AgeGroup)
                   .WithMany(ag => ag.CustomerAppointments)
                   .HasForeignKey(ca => ca.AgeGroupId)
                   .OnDelete(DeleteBehavior.Restrict); // Restrict deletion
            builder.HasOne(e => e.Tenant)
                   .WithMany()
                   .HasForeignKey(e => e.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            var fixedDate = DateTime.Today; // A fixed base date
            var random = new Random(123); // Fixed seed
            
            builder.HasData(
                new CustomerAppointment
                {
                    CustomerAppointmentId = 1,
                    Name = "Alice",
                    Surname = "Smith",
                    Gender = Gender.Female,
                    AgeGroupId = 1,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 30, 0),
                    Price = 150m,
                    // Original time: 10:00, now using today's date plus random days (1 to 3) plus 10:00
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(10).AddMinutes(0),
                    PhoneNumber = "+90 123 456 7891",
                    EMail = "alice.smith@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 2,
                    Name = "Bob",
                    Surname = "Johnson",
                    Gender = Gender.Male,
                    AgeGroupId = 2,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(0, 45, 0),
                    Price = 200m,
                    // Original time: 11:30
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(11).AddMinutes(30),
                    PhoneNumber = "+90 123 456 7892",
                    EMail = "bob.johnson@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 3,
                    Name = "Charlie",
                    Surname = "Brown",
                    Gender = Gender.Male,
                    AgeGroupId = 3,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 60, 0),
                    Price = 250m,
                    // Original time: 14:00
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(14).AddMinutes(0),
                    PhoneNumber = "+90 123 456 7893",
                    EMail = "charlie.brown@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Completed,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 4,
                    Name = "Diana",
                    Surname = "Prince",
                    Gender = Gender.Female,
                    AgeGroupId = 2,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 40, 0),
                    Price = 180m,
                    // Original time: 09:45
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(9).AddMinutes(45),
                    PhoneNumber = "+90 123 456 7894",
                    EMail = "diana.prince@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Cancelled,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 5,
                    Name = "Eve",
                    Surname = "Adams",
                    Gender = Gender.Female,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(0, 35, 0),
                    Price = 160m,
                    // Original time: 16:15
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(16).AddMinutes(15),
                    PhoneNumber = "+90 123 456 7895",
                    EMail = "eve.adams@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 6,
                    Name = "Frank",
                    Surname = "Miller",
                    Gender = Gender.Male,
                    AgeGroupId = 2,
                    EmployeeId = 3,
                    ApproximateDuration = new TimeSpan(0, 30, 0),
                    Price = 120m,
                    // Original time: 12:30
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(12).AddMinutes(30),
                    PhoneNumber = "+90 123 456 7896",
                    EMail = "frank.miller@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 7,
                    Name = "Grace",
                    Surname = "Hall",
                    Gender = Gender.Female,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(1, 15, 0),
                    Price = 450m,
                    // Original time: 15:00
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(15).AddMinutes(0),
                    PhoneNumber = "+90 123 456 7897",
                    EMail = "grace.hall@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 8,
                    Name = "Henry",
                    Surname = "Ford",
                    Gender = Gender.Male,
                    AgeGroupId = 3,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(1, 45, 0),
                    Price = 700m,
                    // Original time: 14:30
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(14).AddMinutes(30),
                    PhoneNumber = "+90 123 456 7898",
                    EMail = "henry.ford@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Completed,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 9,
                    Name = "Isabelle",
                    Surname = "Clark",
                    Gender = Gender.Female,
                    AgeGroupId = 3,
                    EmployeeId = 3,
                    ApproximateDuration = new TimeSpan(0, 45, 0),
                    Price = 250m,
                    // Original time: 10:00
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(10).AddMinutes(0),
                    PhoneNumber = "+90 123 456 7899",
                    EMail = "isabelle.clark@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Cancelled,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 10,
                    Name = "Jack",
                    Surname = "White",
                    Gender = Gender.Male,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(1, 0, 0),
                    Price = 300m,
                    // Original time: 09:15
                    StartDateTime = fixedDate.AddDays(random.Next(1, 4)).AddHours(9).AddMinutes(15),
                    PhoneNumber = "+90 123 456 7890",
                    EMail = "jack.white@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                }
            );
            
        }
    }
}
