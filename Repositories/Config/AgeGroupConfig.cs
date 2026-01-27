using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class AgeGroupConfig : IEntityTypeConfiguration<AgeGroup>
    {
        public void Configure(EntityTypeBuilder<AgeGroup> builder)
        {
            // Use AgeGroupId as the primary key
            builder.HasKey(ag => ag.AgeGroupId);

            // Add a unique constraint on MinAge and MaxAge
            builder.HasIndex(ag => new { ag.MinAge, ag.MaxAge }).IsUnique();

            builder.HasOne(e => e.Tenant)
                   .WithMany()
                   .HasForeignKey(e => e.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            var ageGroup1 = new AgeGroup { AgeGroupId = 1, MinAge = 0, MaxAge = 17 , TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            var ageGroup2 = new AgeGroup { AgeGroupId = 2, MinAge = 18, MaxAge = 75, TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            var ageGroup3 = new AgeGroup { AgeGroupId = 3, MinAge = 76, MaxAge = 125, TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") };
            builder.HasData(ageGroup1, ageGroup2, ageGroup3);
        }
    }
}
