namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IAgeGroupRepository AgeGroupRepository { get; }
        IOfferedServiceRepository OfferedServiceRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        ICustomerAppointmentRepository CustomerAppointmentRepository { get; }
        IBranchRepository BranchRepository { get; }
        //IShopSettingsRepository ShopSettingsRepository { get; }
        IEmployeeLeaveRepository EmployeeLeaveRepository { get; }
        void Save();
    }
}
