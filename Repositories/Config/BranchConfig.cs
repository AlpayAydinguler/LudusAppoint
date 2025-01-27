using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            var branch = new Branch
            {
                BranchId = 1,
                BranchName = "Hacıhalil Şube",
                Country = "Turkey",
                City = "Kocaeli",
                District = "Gebze",
                Neighbourhood = "Hacıhalil",
                Street = "Kızılay caddesi, 1203. Sk.",
                Address = "no 13 a",
                BranchPhoneNumber = "+90 537 025 52 80",
                BranchEmail = "businessmail@business.com",
                Status = true

            };
            var branch2 = new Branch
            {
                BranchId = 2,
                BranchName = "Gebze Şube",
                Country = "Turkey",
                City = "Kocaeli",
                District = "Gebze",
                Neighbourhood = "Osman Yılmaz",
                Street = "Kızılay Cd.",
                Address = "No:68",
                BranchPhoneNumber = "+90 537 025 52 80",
                BranchEmail = "businessmail@business.com",
                Status = false
            };
            builder.HasData(branch,branch2);
        }
    }
}
