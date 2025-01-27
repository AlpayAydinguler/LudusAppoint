using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Repositories.Config
{
    public class AgeGroupConfig : IEntityTypeConfiguration<AgeGroup>
    {
        public void Configure(EntityTypeBuilder<AgeGroup> builder)
        {
            builder.HasAlternateKey(ag => new { ag.MinAge, ag.MaxAge });
            var ageGroup1 = new AgeGroup { AgeGroupId = 1, MinAge = 0, MaxAge = 17 };
            var ageGroup2 = new AgeGroup { AgeGroupId = 2, MinAge = 18, MaxAge = 75 };
            var ageGroup3 = new AgeGroup { AgeGroupId = 3, MinAge = 76, MaxAge = 125 };
            builder.HasData(ageGroup1, ageGroup2, ageGroup3);
        }
    }
}
