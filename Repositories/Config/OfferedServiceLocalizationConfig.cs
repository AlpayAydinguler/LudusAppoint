using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class OfferedServiceLocalizationConfig : IEntityTypeConfiguration<OfferedServiceLocalization>
    {
        public void Configure(EntityTypeBuilder<OfferedServiceLocalization> builder)
        {
            
            builder.HasData(// HairCut
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 1, OfferedServiceId = 1, Language = "en-GB", OfferedServiceLocalizationName = "Hair Cut" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 2, OfferedServiceId = 1, Language = "tr-TR", OfferedServiceLocalizationName = "Saç Kesimi" },
                            // RazorShave
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 3, OfferedServiceId = 2, Language = "en-GB", OfferedServiceLocalizationName = "Razor Shave" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 4, OfferedServiceId = 2, Language = "tr-TR", OfferedServiceLocalizationName = "Jilet Traşı" },
                            // HairColoring
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 5, OfferedServiceId = 3, Language = "en-GB", OfferedServiceLocalizationName = "Hair Coloring" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 6, OfferedServiceId = 3, Language = "tr-TR", OfferedServiceLocalizationName = "Saç Boyama" },
                            // BrowShaping
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 7, OfferedServiceId = 4, Language = "en-GB", OfferedServiceLocalizationName = "Brow Shaping" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 8, OfferedServiceId = 4, Language = "tr-TR", OfferedServiceLocalizationName = "Kaş Şekillendirme" },
                            // BeardGrooming
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 9, OfferedServiceId = 5, Language = "en-GB", OfferedServiceLocalizationName = "Beard Grooming" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 10, OfferedServiceId = 5, Language = "tr-TR", OfferedServiceLocalizationName = "Sakal Bakımı" },
                            // ChildShave
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 11, OfferedServiceId = 6, Language = "en-GB", OfferedServiceLocalizationName = "Child Shave" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 12, OfferedServiceId = 6, Language = "tr-TR", OfferedServiceLocalizationName = "Çocuk Tıraşı" },
                            // PermHair
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 13, OfferedServiceId = 7, Language = "en-GB", OfferedServiceLocalizationName = "Perm Hair" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 14, OfferedServiceId = 7, Language = "tr-TR", OfferedServiceLocalizationName = "Perma Saç" },
                            // Manicure
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 15, OfferedServiceId = 8, Language = "en-GB", OfferedServiceLocalizationName = "Manicure" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 16, OfferedServiceId = 8, Language = "tr-TR", OfferedServiceLocalizationName = "Manikür" },
                            // Pedicure
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 17, OfferedServiceId = 9, Language = "en-GB", OfferedServiceLocalizationName = "Pedicure" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 18, OfferedServiceId = 9, Language = "tr-TR", OfferedServiceLocalizationName = "Pedikür" },
                            // GroomsCut
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 19, OfferedServiceId = 10, Language = "en-GB", OfferedServiceLocalizationName = "Groom's Cut" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 20, OfferedServiceId = 10, Language = "tr-TR", OfferedServiceLocalizationName = "Damat Kesimi" },
                            // Makeup(Bride)
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 21, OfferedServiceId = 11, Language = "en-GB", OfferedServiceLocalizationName = "Makeup (Bride)" },
                            new OfferedServiceLocalization { OfferedServiceLocalizationId = 22, OfferedServiceId = 11, Language = "tr-TR", OfferedServiceLocalizationName = "Makyaj (Gelin)" });
            
        }
    }
}
