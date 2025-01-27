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

            builder.HasData(
                new CustomerAppointment
                {
                    CustomerAppointmentId = 1,
                    Name = "Alice",
                    Surname = "Smith",
                    Gender = Gender.f,
                    AgeGroupId = 1,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 30, 0),
                    Price = 150m,
                    StartDateTime = new DateTime(2025, 1, 5, 10, 0, 0),
                    PhoneNumber = "+90 123 456 7891",
                    EMail = "alice.smith@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1
                },

                new CustomerAppointment
                {
                    CustomerAppointmentId = 2,
                    Name = "Bob",
                    Surname = "Johnson",
                    Gender = Gender.m,
                    AgeGroupId = 2,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(0, 45, 0),
                    Price = 200m,
                    StartDateTime = new DateTime(2025, 1, 6, 11, 30, 0),
                    PhoneNumber = "+90 123 456 7892",
                    EMail = "bob.johnson@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 3,
                    Name = "Charlie",
                    Surname = "Brown",
                    Gender = Gender.m,
                    AgeGroupId = 3,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 60, 0),
                    Price = 250m,
                    StartDateTime = new DateTime(2025, 1, 7, 14, 0, 0),
                    PhoneNumber = "+90 123 456 7893",
                    EMail = "charlie.brown@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Completed,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 4,
                    Name = "Diana",
                    Surname = "Prince",
                    Gender = Gender.f,
                    AgeGroupId = 2,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(0, 40, 0),
                    Price = 180m,
                    StartDateTime = new DateTime(2025, 1, 8, 9, 45, 0),
                    PhoneNumber = "+90 123 456 7894",
                    EMail = "diana.prince@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Cancelled,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 5,
                    Name = "Eve",
                    Surname = "Adams",
                    Gender = Gender.f,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(0, 35, 0),
                    Price = 160m,
                    StartDateTime = new DateTime(2025, 1, 9, 16, 15, 0),
                    PhoneNumber = "+90 123 456 7895",
                    EMail = "eve.adams@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 6,
                    Name = "Frank",
                    Surname = "Miller",
                    Gender = Gender.m,
                    AgeGroupId = 2,
                    EmployeeId = 3,
                    ApproximateDuration = new TimeSpan(0, 30, 0),
                    Price = 120m,
                    StartDateTime = new DateTime(2025, 1, 10, 12, 30, 0),
                    PhoneNumber = "+90 123 456 7896",
                    EMail = "frank.miller@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 7,
                    Name = "Grace",
                    Surname = "Hall",
                    Gender = Gender.f,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(1, 15, 0),
                    Price = 450m,
                    StartDateTime = new DateTime(2025, 1, 11, 15, 0, 0),
                    PhoneNumber = "+90 123 456 7897",
                    EMail = "grace.hall@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Confirmed,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 8,
                    Name = "Henry",
                    Surname = "Ford",
                    Gender = Gender.m,
                    AgeGroupId = 3,
                    EmployeeId = 1,
                    ApproximateDuration = new TimeSpan(1, 45, 0),
                    Price = 700m,
                    StartDateTime = new DateTime(2025, 1, 12, 14, 30, 0),
                    PhoneNumber = "+90 123 456 7898",
                    EMail = "henry.ford@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Completed,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 9,
                    Name = "Isabelle",
                    Surname = "Clark",
                    Gender = Gender.f,
                    AgeGroupId = 3,
                    EmployeeId = 3,
                    ApproximateDuration = new TimeSpan(0, 45, 0),
                    Price = 250m,
                    StartDateTime = new DateTime(2025, 1, 13, 10, 0, 0),
                    PhoneNumber = "+90 123 456 7899",
                    EMail = "isabelle.clark@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.Cancelled,
                    BranchId = 1
                },
                new CustomerAppointment
                {
                    CustomerAppointmentId = 10,
                    Name = "Jack",
                    Surname = "White",
                    Gender = Gender.m,
                    AgeGroupId = 1,
                    EmployeeId = 2,
                    ApproximateDuration = new TimeSpan(1, 0, 0),
                    Price = 300m,
                    StartDateTime = new DateTime(2025, 1, 14, 9, 15, 0),
                    PhoneNumber = "+90 123 456 7890",
                    EMail = "jack.white@example.com",
                    CreatedBy = null,
                    Status = CustomerAppointmentStatus.AwaitingApproval,
                    BranchId = 1
                }
            );
        }
    }
}
