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
        IAccountService AccountService { get; }
        IAuthService AuthService { get; }
        IApplicationUserService UserService { get; }
        ITenantService TenantService { get; }
        IBookingFlowConfigService BookingFlowConfigService { get; }
    }
}
