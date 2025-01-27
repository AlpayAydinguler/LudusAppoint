using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repositories
{
    public class RepositoryContext : DbContext
    {
        public DbSet<AgeGroup> AgeGroups { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<OfferedService> OfferedServices { get; set; }

        public DbSet<CustomerAppointment> CustomerAppointments { get; set; }
        public DbSet<OfferedServiceLocalization> OfferedServiceLocalizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ShopSettings> ShopSettings { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerAppointment>().HasOne(c => c.Employee)
                                                      .WithMany(h => h.CustomerAppointment)
                                                      .HasForeignKey(c => c.EmployeeId)
                                                      .OnDelete(DeleteBehavior.NoAction);
            /*
            modelBuilder.Entity<OfferedServiceLocalization>().HasOne(l => l.OfferedService)
                                                             .WithMany(h => h.OfferedServiceLocalizations)
                                                             .HasForeignKey(l => l.OfferedServiceId)
                                                             .OnDelete(DeleteBehavior.Cascade);
            */

            /*
            modelBuilder.Entity<OfferedService>().HasMany(x => x.AgeGroups)
                                                 .WithMany(y => y.OfferedServices)
                                                 .UsingEntity("OfferedServiceAgeGroups");

            modelBuilder.Entity<Employee>().HasMany(x => x.OfferedServices)
                                           .WithMany(y => y.Employees)
                                           .UsingEntity("EmployeeOfferedServices");

            modelBuilder.Entity<CustomerAppointment>().HasOne(c => c.Employee)
                                                      .WithMany(h => h.CustomerAppointment)
                                                      .HasForeignKey(c => c.EmployeeId)
                                                      .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(ag => ag.AgeGroups)
                   .WithMany(os => os.OfferedServices)
                   .UsingEntity<Dictionary<string, object>>("OfferedServiceAgeGroups",
                                                           b => b.HasOne<AgeGroup>().WithMany().HasForeignKey("AgeGroupId"),
                                                           b => b.HasOne<OfferedService>().WithMany().HasForeignKey("OfferedServiceId"),
                                                           b =>
                                                           {
                                                               b.Property<int>("AgeGroupId");
                                                               b.Property<int>("OfferedServiceId");
                                                               b.HasKey("AgeGroupId", "OfferedServiceId");
                                                           });
            */
            SeedingData(modelBuilder);
        }

        private void SeedingData(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity("OfferedServiceAgeGroups").HasData(
                new { OfferedServiceId = 1, AgeGroupsId = 1 },
                new { OfferedServiceId = 2, AgeGroupsId = 2 },
                new { OfferedServiceId = 3, AgeGroupsId = 1 },
                new { OfferedServiceId = 3, AgeGroupsId = 2 },
                new { OfferedServiceId = 4, AgeGroupsId = 1 },
                new { OfferedServiceId = 5, AgeGroupsId = 1 },
                new { OfferedServiceId = 5, AgeGroupsId = 2 },
                new { OfferedServiceId = 6, AgeGroupsId = 1 },
                new { OfferedServiceId = 7, AgeGroupsId = 2 },
                new { OfferedServiceId = 8, AgeGroupsId = 2 },
                new { OfferedServiceId = 9, AgeGroupsId = 2 },
                new { OfferedServiceId = 10, AgeGroupsId = 2 },
                new { OfferedServiceId = 11, AgeGroupsId = 2 }
            );

            modelBuilder.Entity("EmployeeOfferedServices").HasData(
                new { EmployeeId = 1, OfferedServicesId = 1 },
                new { EmployeeId = 1, OfferedServicesId = 2 },
                new { EmployeeId = 1, OfferedServicesId = 3 },
                new { EmployeeId = 1, OfferedServicesId = 4 },
                new { EmployeeId = 1, OfferedServicesId = 5 },
                new { EmployeeId = 2, OfferedServicesId = 2 },
                new { EmployeeId = 1, OfferedServicesId = 6 },
                new { EmployeeId = 1, OfferedServicesId = 7 },
                new { EmployeeId = 1, OfferedServicesId = 8 },
                new { EmployeeId = 1, OfferedServicesId = 9 },
                new { EmployeeId = 1, OfferedServicesId = 10 }
            );

            modelBuilder.Entity("CustomerAppointmentOfferedServices").HasData(
                new { CustomerAppointmentId = 1, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 1, OfferedServicesId = 3 },
                new { CustomerAppointmentId = 2, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 2, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 2, OfferedServicesId = 3 },
                new { CustomerAppointmentId = 3, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 4, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 4, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 5, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 5, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 6, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 6, OfferedServicesId = 3 },
                new { CustomerAppointmentId = 7, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 8, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 8, OfferedServicesId = 4 },
                new { CustomerAppointmentId = 8, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 9, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 10, OfferedServicesId = 2 },
                new { CustomerAppointmentId = 10, OfferedServicesId = 1 },
                new { CustomerAppointmentId = 10, OfferedServicesId = 3 }
            );

        }

    }
}
