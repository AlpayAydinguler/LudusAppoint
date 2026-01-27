using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TenantManager : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<TenantManager> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantManager(IMapper mapper,
                             IRepositoryManager repositoryManager,
                             IStringLocalizer<TenantManager> localizer,
                             IHttpContextAccessor httpContextAccessor )
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateTenantAsync(TenantDtoForInsert tenantDto)
        {
            var validationException = new List<ValidationException>();

            var existingTenant = await _repositoryManager.TenantRepository
                .FindByConditionAsync(t => t.Name == tenantDto.Name || t.Hostname == tenantDto.Hostname, false);

            if (existingTenant != null)
            {
                validationException.Add(new ValidationException(_localizer["TenantNameOrHostnameAlreadyExists"],
                    new Exception() { Source = "Model" }));
            }
            else
            {
                var tenant = _mapper.Map<Tenant>(tenantDto);
                await _repositoryManager.TenantRepository.CreateAsync(tenant);
                // await _repositoryManager.TenantRepository.SaveAsync();
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task DeleteTenantAsync(Guid id)
        {
            var validationException = new List<ValidationException>();

            var tenant = await _repositoryManager.TenantRepository.FindByConditionAsync(t => t.Id == id, false);
            if (tenant == null)
            {
                validationException.Add(new ValidationException(_localizer["TenantNotFound"],
                    new Exception() { Source = "Model" }));
            }
            else
            {
                await _repositoryManager.TenantRepository.DeleteAsync(tenant);
                //await _repositoryManager.TenantRepository.SaveAsync();
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task UpdateTenantAsync(TenantDtoForUpdate tenantDto)
        {
            var validationException = new List<ValidationException>();

            var existingTenant = await _repositoryManager.TenantRepository
                .FindByConditionAsync(t => (t.Name == tenantDto.Name || t.Hostname == tenantDto.Hostname) && t.Id != tenantDto.Id, false);

            if (existingTenant != null)
            {
                validationException.Add(new ValidationException(_localizer["TenantNameOrHostnameAlreadyExists"],
                    new Exception() { Source = "Model" }));
            }
            else
            {
                var tenant = await _repositoryManager.TenantRepository.FindByConditionAsync(t => t.Id == tenantDto.Id, true);
                if (tenant == null)
                {
                    validationException.Add(new ValidationException(_localizer["TenantNotFound"],
                        new Exception() { Source = "Model" }));
                }
                else
                {
                    _mapper.Map(tenantDto, tenant);
                    tenant.UpdatedAt = DateTime.UtcNow;
                    // _repositoryManager.TenantRepository.Update(tenant);
                    await _repositoryManager.TenantRepository.SaveAsync();
                }
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task<TenantDto?> GetTenantByIdAsync(Guid id)
        {
            var tenant = await _repositoryManager.TenantRepository.FindByConditionAsync(t => t.Id == id, false);
            return _mapper.Map<TenantDto>(tenant);
        }
        public async Task<Tenant?> GetTenantByHostnameAsync(string hostname)
        {
            return await _repositoryManager.TenantRepository.GetTenantByHostnameAsync(hostname, false);
        }

        public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync(bool trackChanges = false)
        {
            var tenants = await _repositoryManager.TenantRepository.GetAllAsync(trackChanges);
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }
        public async Task<Tenant?> GetCurrentTenantAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items.TryGetValue("CurrentTenant", out var tenantObj) == true)
            {
                return tenantObj as Tenant;
            }

            // Fallback: Try to get from hostname
            var host = httpContext?.Request.Host.Host;
            if (!string.IsNullOrEmpty(host))
            {
                return await GetTenantByHostnameAsync(host);
            }

            return null;
        }
    }
}
