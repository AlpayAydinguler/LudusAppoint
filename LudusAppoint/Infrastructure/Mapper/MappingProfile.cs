using AutoMapper;
using Entities.Dtos;
using Entities.Models;

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
        }
    }
}
