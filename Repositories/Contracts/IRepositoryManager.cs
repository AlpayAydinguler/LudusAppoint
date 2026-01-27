
namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IAgeGroupRepository AgeGroupRepository { get; }
        IOfferedServiceRepository OfferedServiceRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        ICustomerAppointmentRepository CustomerAppointmentRepository { get; }
        IBranchRepository BranchRepository { get; }
        IEmployeeLeaveRepository EmployeeLeaveRepository { get; }
        IApplicationSettingRepository ApplicationSettingRepository { get; }
        ITenantRepository TenantRepository { get; }
        IBookingFlowConfigRepository BookingFlowConfigRepository { get; }

        Task BeginTransactionAsync();
        void Save();
    }
}
