using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class OfferedServiceLocalizationConfig : IEntityTypeConfiguration<OfferedServiceLocalization>
    {
        public void Configure(EntityTypeBuilder<OfferedServiceLocalization> builder)
        {
            builder.HasOne(e => e.Tenant)
                   .WithMany()
                   .HasForeignKey(e => e.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(// HairCut
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 1, OfferedServiceId = 1, Language = "en-GB", OfferedServiceLocalizationName = "Hair Cut", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 2, OfferedServiceId = 1, Language = "tr-TR", OfferedServiceLocalizationName = "Saç Kesimi", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // RazorShave
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 3, OfferedServiceId = 2, Language = "en-GB", OfferedServiceLocalizationName = "Razor Shave", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 4, OfferedServiceId = 2, Language = "tr-TR", OfferedServiceLocalizationName = "Jilet Traşı", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // HairColoring
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 5, OfferedServiceId = 3, Language = "en-GB", OfferedServiceLocalizationName = "Hair Coloring", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 6, OfferedServiceId = 3, Language = "tr-TR", OfferedServiceLocalizationName = "Saç Boyama", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // BrowShaping
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 7, OfferedServiceId = 4, Language = "en-GB", OfferedServiceLocalizationName = "Brow Shaping", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 8, OfferedServiceId = 4, Language = "tr-TR", OfferedServiceLocalizationName = "Kaş Şekillendirme", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // BeardGrooming
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 9, OfferedServiceId = 5, Language = "en-GB", OfferedServiceLocalizationName = "Beard Grooming", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 10, OfferedServiceId = 5, Language = "tr-TR", OfferedServiceLocalizationName = "Sakal Bakımı", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // ChildShave
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 11, OfferedServiceId = 6, Language = "en-GB", OfferedServiceLocalizationName = "Child Shave", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 12, OfferedServiceId = 6, Language = "tr-TR", OfferedServiceLocalizationName = "Çocuk Tıraşı", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // PermHair
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 13, OfferedServiceId = 7, Language = "en-GB", OfferedServiceLocalizationName = "Perm Hair", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 14, OfferedServiceId = 7, Language = "tr-TR", OfferedServiceLocalizationName = "Perma Saç", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // Manicure
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 15, OfferedServiceId = 8, Language = "en-GB", OfferedServiceLocalizationName = "Manicure", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 16, OfferedServiceId = 8, Language = "tr-TR", OfferedServiceLocalizationName = "Manikür", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // Pedicure
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 17, OfferedServiceId = 9, Language = "en-GB", OfferedServiceLocalizationName = "Pedicure", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 18, OfferedServiceId = 9, Language = "tr-TR", OfferedServiceLocalizationName = "Pedikür", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // GroomsCut
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 19, OfferedServiceId = 10, Language = "en-GB", OfferedServiceLocalizationName = "Groom's Cut", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 20, OfferedServiceId = 10, Language = "tr-TR", OfferedServiceLocalizationName = "Damat Kesimi", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            // Makeup(Bride)
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 21, OfferedServiceId = 11, Language = "en-GB", OfferedServiceLocalizationName = "Makeup (Bride)", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 22, OfferedServiceId = 11, Language = "tr-TR", OfferedServiceLocalizationName = "Makyaj (Gelin)", TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111") });
            
        }
    }
}
