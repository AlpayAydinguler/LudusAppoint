using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IAgeGroupService _ageGroupService;
        private readonly IEmployeeService _employeeService;
        private readonly IOfferedServiceService _offeredServiceService;
        private readonly ICustomerAppointmentService _customerAppointmentService;
        private readonly IBranchService _branchService;
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IApplicationSettingService _applicationSettingService;

        public ServiceManager(IAgeGroupService ageGroupService, IEmployeeService employeeService, IOfferedServiceService offeredServiceService, ICustomerAppointmentService customerAppointmentService, IBranchService branchService, IEmployeeLeaveService employeeLeaveService, IApplicationSettingService applicationSettingService)
        {
            _ageGroupService = ageGroupService;
            _employeeService = employeeService;
            _offeredServiceService = offeredServiceService;
            _customerAppointmentService = customerAppointmentService;
            _branchService = branchService;
            _employeeLeaveService = employeeLeaveService;
            _applicationSettingService = applicationSettingService;
        }

        public IAgeGroupService AgeGroupService => _ageGroupService;
        public IEmployeeService EmployeeService => _employeeService;
        public IOfferedServiceService OfferedServiceService => _offeredServiceService;
        public ICustomerAppointmentService CustomerAppointmentService => _customerAppointmentService;
        public IBranchService BranchService => _branchService;
        public IEmployeeLeaveService EmployeeLeaveService => _employeeLeaveService;
        public IApplicationSettingService ApplicationSettingService => _applicationSettingService;
    }
}
