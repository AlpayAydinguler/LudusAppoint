using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class ApplicationSettingConfig : IEntityTypeConfiguration<ApplicationSetting>
    {
        public void Configure(EntityTypeBuilder<ApplicationSetting> builder)
        {
            
            builder.HasData(
                            new ApplicationSetting { Key = "SupportedGenders", Value = "Male,Female" },
                            new ApplicationSetting { Key = "CompanyName", Value = "Hair Center" },
                            new ApplicationSetting { Key = "CompanyLogoURL", Value = "\\assets\\img\\logo.jpg" },
                            new ApplicationSetting { Key = "Currency", Value = "tr-TR" }
                        );
            
        }
    }
}
