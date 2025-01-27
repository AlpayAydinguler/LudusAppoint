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

            CreateMap<CustomerAppointmentForInsert, CustomerAppointment>();
            CreateMap<CustomerAppointmentForUpdate, CustomerAppointment>().ReverseMap();
            CreateMap<CustomerAppointment, CustomerAppointmentDto>();

            CreateMap<EmployeeDtoForInsert, Employee>();
            CreateMap<EmployeeDtoForUpdate, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeLeaveDtoForInsert, EmployeeLeave>();
            CreateMap<EmployeeLeaveDtoForUpdate, EmployeeLeave>().ReverseMap();
            CreateMap<EmployeeLeave, EmployeeLeaveDto>();

            CreateMap<OfferedServiceDtoForInsert, OfferedService>();
            CreateMap<OfferedServiceDtoForUpdate, OfferedService>().ReverseMap();
            CreateMap<OfferedService, OfferedServiceDto>();

            CreateMap<OfferedServiceLocalizationDtoForInsert, OfferedServiceLocalization>();
            CreateMap<OfferedServiceLocalizationDtoForUpdate, OfferedServiceLocalization>().ReverseMap();
            CreateMap<OfferedServiceLocalization, OfferedServiceLocalizationDto>();
        }
    }
}
