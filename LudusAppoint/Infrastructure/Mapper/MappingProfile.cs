using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace LudusAppoint.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AgeGroupDtoForInsert, AgeGroup>();
            CreateMap<AgeGroupDtoForUpdate, AgeGroup>().ReverseMap();
            CreateMap<AgeGroup, AgeGroupDto>();

            CreateMap<BranchDtoForInsert, Branch>();
            CreateMap<BranchDtoForUpdate, Branch>().ReverseMap();
            CreateMap<Branch, BranchDto>();

            CreateMap<CustomerAppointmentDtoForInsert, CustomerAppointment>();
            CreateMap<CustomerAppointmentDtoForUpdate, CustomerAppointment>().ReverseMap()
                                                                             .ForMember(dest => dest.OfferedServiceIds, opt => opt.MapFrom(src => src.OfferedServices.Select(os => os.OfferedServiceId)));
            CreateMap<CustomerAppointment, CustomerAppointmentDto>();

            CreateMap<EmployeeDtoForInsert, Employee>();
            CreateMap<EmployeeDtoForUpdate, Employee>().ReverseMap()
                                                       .ForMember(dest => dest.OfferedServiceIds, opt => opt.MapFrom(src => src.OfferedServices.Select(os => os.OfferedServiceId)));
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeLeaveDtoForInsert, EmployeeLeave>();
            CreateMap<EmployeeLeaveDtoForUpdate, EmployeeLeave>().ReverseMap();
            CreateMap<EmployeeLeave, EmployeeLeaveDto>();

            CreateMap<OfferedServiceDtoForInsert, OfferedService>();
            CreateMap<OfferedServiceDtoForUpdate, OfferedService>().ReverseMap()
                                                                   .ForMember(dest => dest.AgeGroupIds, opt => opt.MapFrom(src => src.AgeGroups.Select(ag => ag.AgeGroupId)));
            CreateMap<OfferedService, OfferedServiceDto>();

            CreateMap<OfferedServiceLocalizationDtoForInsert, OfferedServiceLocalization>();
            CreateMap<OfferedServiceLocalizationDtoForUpdate, OfferedServiceLocalization>().ReverseMap();
            CreateMap<OfferedServiceLocalization, OfferedServiceLocalizationDto>();

            CreateMap<ApplicationSettingDtoForInsert, ApplicationSetting>();
            CreateMap<ApplicationSettingDtoForUpdate, ApplicationSetting>().ReverseMap();
            CreateMap<ApplicationSetting, ApplicationSettingDto>();

            CreateMap<UserDtoForInsert, ApplicationUser>();
            CreateMap<UserDtoForUpdate, ApplicationUser>().ReverseMap()
                                                          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ApplicationUser, UserDto>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                                                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<RoleDtoForInsert, IdentityRole>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.RoleId) ? Guid.NewGuid().ToString() : src.RoleId))
                                                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                                                       .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.RoleName.ToUpper()));
            CreateMap<RoleDtoForUpdate, IdentityRole>().ReverseMap()
                                                       .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                                                       .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<IdentityRole, RoleDto>().ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                                              .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<TenantDtoForInsert, Tenant>();
            CreateMap<TenantDtoForUpdate, Tenant>().ReverseMap();
            CreateMap<Tenant, TenantDto>();
            CreateMap<BookingFlowConfigDtoForInsert, BookingFlowConfig>();
            CreateMap<BookingFlowConfigDtoForUpdate, BookingFlowConfig>().ReverseMap();
            CreateMap<BookingFlowConfig, BookingFlowConfigDto>();
        }
    }
}
