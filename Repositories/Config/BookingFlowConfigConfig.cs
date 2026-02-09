using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class BookingFlowConfigConfig : IEntityTypeConfiguration<BookingFlowConfig>
    {
        public void Configure(EntityTypeBuilder<BookingFlowConfig> builder)
        {
            builder.HasKey(b => b.BookingFlowConfigId);

            builder.Property(b => b.AllStepsInOrder)
                .IsRequired()
                .HasDefaultValue("[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]");

            builder.Property(b => b.EnabledStepsInOrder)
                .IsRequired()
                .HasDefaultValue("[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]");

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships - CHANGE CASCADE BEHAVIOR
            builder.HasOne(b => b.Tenant)
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

            builder.HasOne(b => b.Branch)
                .WithMany()
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Branch

            // Optional: Add a unique constraint for one config per branch per tenant
            builder.HasIndex(b => new { b.TenantId, b.BranchId })
                .IsUnique();
        }
    }
}