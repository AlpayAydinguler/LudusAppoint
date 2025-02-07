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

        public void AttachAsUnchanged(OfferedService offeredService)
        {
            _repositoryContext.Entry(offeredService).State = EntityState.Unchanged;
        }

        public void CreateofferedService(OfferedService offeredService)
        {
            Create(offeredService);
        }

        public IEnumerable<OfferedService> GetActiveOfferedServices(bool trackChanges, string language)
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.AgeGroups)
                .Include(h => h.OfferedServiceLocalizations)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return offeredServices
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
                .ToList();
        }

        public IEnumerable<OfferedService> GetAllForCustomerAppointment(Gender gender, int ageGroupId, bool trackChanges, string language)
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.OfferedServiceLocalizations)
                .Include(h => h.AgeGroups)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return offeredServices
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
                .ToList();
        }

        public List<OfferedService> GetAllOfferedServices(bool trackChanges, string language = "en-GB")
        {
            var offeredServices = _repositoryContext.OfferedServices
                .Include(h => h.AgeGroups)
                .Include(h => h.OfferedServiceLocalizations)
                .AsQueryable();

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return offeredServices
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
                .ToList();
        }

        public OfferedService GetofferedService(int id, bool trackChanges)
        {
            return FindByCondition(x => x.OfferedServiceId.Equals(id), trackChanges) ?? new OfferedService();
        }

        public int GetMinApproximateDuration()
        {
            var minDuration = _repositoryContext.OfferedServices.OrderBy(hs => hs.ApproximateDuration)
                                                                     .AsNoTracking()
                                                                     .Select(hs => hs.ApproximateDuration.TotalMinutes)
                                                                     .First();
            return (int)minDuration;
            
        }

        public OfferedService GetOfferedServiceForUpdate(int id, bool trackChanges)
        {
            var offeredServices = _repositoryContext.OfferedServices.Include(h => h.AgeGroups)
                                                          .Include(h => h.OfferedServiceLocalizations)
                                                          .Where(h => h.OfferedServiceId == id);  // Add filter by ID
        

            if (!trackChanges)
            {
                offeredServices = offeredServices.AsNoTracking();
            }

            return offeredServices.FirstOrDefault();
        }

        public void Update(OfferedService offeredService, List<int> ageGroupIds)
        {
            offeredService.AgeGroups.Clear();

            foreach (var id in ageGroupIds)
            {
                var ageGroup = _repositoryContext.AgeGroups
                    .FirstOrDefault(ag => ag.AgeGroupId == id);

                offeredService.AgeGroups.Add(ageGroup);
            }
        }
    }
}
