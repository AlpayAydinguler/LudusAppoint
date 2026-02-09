using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class ApplicationSettingRepository : RepositoryBase<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
