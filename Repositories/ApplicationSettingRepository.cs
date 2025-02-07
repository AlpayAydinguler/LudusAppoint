using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ApplicationSettingRepository : RepositoryBase<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
