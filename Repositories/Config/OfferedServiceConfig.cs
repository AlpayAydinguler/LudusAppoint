using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class OfferedServiceConfig : IEntityTypeConfiguration<OfferedService>
    {
        public void Configure(EntityTypeBuilder<OfferedService> builder)
        {
            builder.HasMany(ag => ag.AgeGroups)
                   .WithMany(os => os.OfferedServices)
                   .UsingEntity<Dictionary<string, object>>("OfferedServiceAgeGroups",
                                                            b => b.HasOne<AgeGroup>().WithMany().HasForeignKey("AgeGroupsId").OnDelete(DeleteBehavior.Restrict),
                                                            b => b.HasOne<OfferedService>().WithMany().HasForeignKey("OfferedServiceId").OnDelete(DeleteBehavior.Restrict),
                                                            b =>
                                                            {
                                                                b.Property<int>("AgeGroupsId");
                                                                b.Property<int>("OfferedServiceId");
                                                                b.HasKey("AgeGroupsId", "OfferedServiceId");
                                                            });

            var offeredService1 = new OfferedService { OfferedServiceId = 1, OfferedServiceName = "HairCut", Genders = { Gender.m }, ApproximateDuration = new TimeSpan(0, 20, 0), Price = 100, Status = true };
            var offeredService2 = new OfferedService { OfferedServiceId = 2, OfferedServiceName = "RazorShave", Genders = { Gender.m }, ApproximateDuration = new TimeSpan(0, 40, 0), Price = 200, Status = true };
            var offeredService3 = new OfferedService { OfferedServiceId = 3, OfferedServiceName = "HairColoring", Genders = { Gender.m }, ApproximateDuration = new TimeSpan(0, 60, 0), Price = 300, Status = true };
            var offeredService4 = new OfferedService { OfferedServiceId = 4, OfferedServiceName = "BrowShaping", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 80, 0), Price = 400, Status = true };
            var offeredService5 = new OfferedService { OfferedServiceId = 5, OfferedServiceName = "BeardGrooming", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 100, 0), Price = 500, Status = true };
            var offeredService6 = new OfferedService { OfferedServiceId = 6, OfferedServiceName = "ChildShave", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 120, 0), Price = 600, Status = true };
            var offeredService7 = new OfferedService { OfferedServiceId = 7, OfferedServiceName = "PermHair", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 140, 0), Price = 700, Status = false };
            var offeredService8 = new OfferedService { OfferedServiceId = 8, OfferedServiceName = "Manicure", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 150, 0), Price = 800, Status = false };
            var offeredService9 = new OfferedService { OfferedServiceId = 9, OfferedServiceName = "Pedicure", Genders = { Gender.m, Gender.f }, ApproximateDuration = new TimeSpan(0, 160, 0), Price = 900, Status = false };
            var offeredService10 = new OfferedService { OfferedServiceId = 10, OfferedServiceName = "GroomsCut", Genders = { Gender.m }, ApproximateDuration = new TimeSpan(0, 180, 0), Price = 1000, Status = true };
            var offeredService11 = new OfferedService { OfferedServiceId = 11, OfferedServiceName = "Makeup(Bride)", Genders = { Gender.f }, ApproximateDuration = new TimeSpan(0, 200, 0), Price = 1100, Status = false };

            List<OfferedService> offeredServices = new List<OfferedService>();
            offeredServices.Add(offeredService1);
            offeredServices.Add(offeredService2);
            offeredServices.Add(offeredService3);
            offeredServices.Add(offeredService4);
            offeredServices.Add(offeredService5);
            offeredServices.Add(offeredService6);
            offeredServices.Add(offeredService7);
            offeredServices.Add(offeredService8);
            offeredServices.Add(offeredService9);
            offeredServices.Add(offeredService10);
            offeredServices.Add(offeredService11);

            builder.HasData(offeredServices);
        }
    }
}
