using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Contracts;

namespace Repositories
{
    public class OfferedServiceRepository : RepositoryBase<OfferedService>, IOfferedServiceRepository
    {
        public OfferedServiceRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AttachAsUnchangedAsync(OfferedService offeredService)
        {
            _repositoryContext.Entry(offeredService).State = EntityState.Unchanged;
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<OfferedService>> GetActiveOfferedServicesAsync(bool trackChanges, string language)
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.AgeGroups)
                .Include(h => h.OfferedServiceLocalizations)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return await offeredServices
                .Select(h => new OfferedService
                {
                    OfferedServiceId = h.OfferedServiceId,
                    OfferedServiceName = h.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language).OfferedServiceLocalizationName ?? h.OfferedServiceName,
                    Genders = h.Genders,
                    ApproximateDuration = h.ApproximateDuration,
                    Price = h.Price,
                    Status = h.Status,
                    AgeGroups = h.AgeGroups
                })
                .Where(h => h.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<OfferedService>> GetAllForCustomerAppointmentAsync(Gender gender, int ageGroupId, bool trackChanges, string language)
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.OfferedServiceLocalizations)
                .Include(h => h.AgeGroups)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return await offeredServices
                .Where(h => h.AgeGroups.Any(ag => ag.AgeGroupId == ageGroupId) &&
                                            h.Genders.Contains(gender) &&
                                            h.Status)
                .Select(h => new OfferedService
                {
                    OfferedServiceId = h.OfferedServiceId,
                    OfferedServiceName = h.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language).OfferedServiceLocalizationName ?? h.OfferedServiceName,
                    Genders = h.Genders,
                    ApproximateDuration = h.ApproximateDuration,
                    Price = h.Price,
                })
                .ToListAsync();
        }

        public async Task<List<OfferedService>> GetAllOfferedServicesAsync(bool trackChanges, string language = "en-GB")
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.AgeGroups)
                .Include(h => h.OfferedServiceLocalizations)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return await offeredServices
                .Select(h => new OfferedService
                {
                    OfferedServiceId = h.OfferedServiceId,
                    OfferedServiceName = h.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language).OfferedServiceLocalizationName ?? h.OfferedServiceName,
                    Genders = h.Genders,
                    ApproximateDuration = h.ApproximateDuration,
                    Price = h.Price,
                    Status = h.Status,
                    AgeGroups = h.AgeGroups
                })
                .ToListAsync();
        }

        public async Task<OfferedService> GetofferedServiceAsync(int id, bool trackChanges)
        {
            return await FindByConditionAsync(x => x.OfferedServiceId.Equals(id), trackChanges) ?? new OfferedService();
        }

        public async Task<int> GetMinApproximateDurationAsync()
        {
            var minDuration = await _repositoryContext.OfferedServices.OrderBy(hs => hs.ApproximateDuration)
                                                                     .AsNoTracking()
                                                                     .Select(hs => hs.ApproximateDuration.TotalMinutes)
                                                                     .FirstAsync();
            return (int)minDuration;
        }

        public async Task<OfferedService> GetOfferedServiceForUpdateAsync(int id, bool trackChanges)
        {
            var offeredServices = _repositoryContext.OfferedServices.Include(h => h.AgeGroups)
                                                          .Include(h => h.OfferedServiceLocalizations)
                                                          .Where(h => h.OfferedServiceId == id);

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return await offeredServices.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(OfferedService offeredService, List<int> ageGroupIds)
        {
            offeredService.AgeGroups.Clear();

            foreach (var id in ageGroupIds)
            {
                var ageGroup = await _repositoryContext.AgeGroups
                    .FirstOrDefaultAsync(ag => ag.AgeGroupId == id);

                offeredService.AgeGroups.Add(ageGroup);
            }
        }
    }
}
