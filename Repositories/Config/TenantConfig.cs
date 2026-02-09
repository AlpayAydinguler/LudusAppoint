using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class TenantConfig : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // Seed a default tenant and two example tenants.
            // Note: HasData requires fixed values for Guid/DateTime.
            var defaultTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var company1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var company2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            builder.HasData(
                new Tenant
                {
                    Id = defaultTenantId,
                    Name = "Default Tenant",
                    Hostname = "localhost",
                    CreatedAt = new DateTime(2023, 11, 17, 11, 37, 29, DateTimeKind.Utc)
                },
                new Tenant
                {
                    Id = company1Id,
                    Name = "Company1",
                    Hostname = "company1.com",
                    CreatedAt = new DateTime(2024, 11, 17, 11, 37, 29, DateTimeKind.Utc)
                },
                new Tenant
                {
                    Id = company2Id,
                    Name = "Company2",
                    Hostname = "company2.com",
                    CreatedAt = new DateTime(2025, 11, 17, 11, 37, 29, DateTimeKind.Utc)
                }
            );
        }
    }
}
