namespace Services.Contracts
{
    public interface IServiceManager
    {
        IAgeGroupService AgeGroupService { get; }
        IEmployeeService EmployeeService { get; }
        IOfferedServiceService OfferedServiceService { get; }
        ICustomerAppointmentService CustomerAppointmentService { get; }
        IBranchService BranchService { get; }
        IEmployeeLeaveService EmployeeLeaveService { get; }
        IApplicationSettingService ApplicationSettingService { get; }
    }
}
