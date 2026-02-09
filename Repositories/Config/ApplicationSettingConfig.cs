using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class ApplicationSettingConfig : IEntityTypeConfiguration<ApplicationSetting>
    {
        public void Configure(EntityTypeBuilder<ApplicationSetting> builder)
        {
            builder.HasOne(e => e.Tenant)
                   .WithMany()
                   .HasForeignKey(e => e.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                            new ApplicationSetting { Key = "SupportedGenders", Value = "Male,Female", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new ApplicationSetting { Key = "CompanyName", Value = "Hair Center", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new ApplicationSetting { Key = "CompanyLogoURL", Value = "\\assets\\img\\logo.jpg", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new ApplicationSetting { Key = "Currency", Value = "tr-TR", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") }
                        );

        }
    }
}
