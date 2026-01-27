using Repositories.Contracts;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly IAgeGroupRepository _ageGroupRepository;
        private readonly IOfferedServiceRepository _offeredServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerAppointmentRepository _customerAppointmentRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IBookingFlowConfigRepository _bookingFlowConfigRepository;

        public RepositoryManager(IAgeGroupRepository ageGroupRepository,
                                 RepositoryContext repositoryContext,
                                 IOfferedServiceRepository offeredServiceRepository,
                                 IEmployeeRepository employeeRepository,
                                 ICustomerAppointmentRepository customerAppointmentRepository,
                                 IBranchRepository branchRepository,
                                 IEmployeeLeaveRepository employeeLeaveRepository,
                                 IApplicationSettingRepository applicationSettingRepository,
                                 ITenantRepository tenantRepository,
                                 IBookingFlowConfigRepository bookingFlowConfigRepository)
        {
            _ageGroupRepository = ageGroupRepository;
            _repositoryContext = repositoryContext;
            _offeredServiceRepository = offeredServiceRepository;
            _employeeRepository = employeeRepository;
            _customerAppointmentRepository = customerAppointmentRepository;
            _branchRepository = branchRepository;
            _employeeLeaveRepository = employeeLeaveRepository;
            _applicationSettingRepository = applicationSettingRepository;
            _tenantRepository = tenantRepository;
            _bookingFlowConfigRepository = bookingFlowConfigRepository;
        }

        public IAgeGroupRepository AgeGroupRepository => _ageGroupRepository;
        public IOfferedServiceRepository OfferedServiceRepository => _offeredServiceRepository;
        public IEmployeeRepository EmployeeRepository => _employeeRepository;
        public ICustomerAppointmentRepository CustomerAppointmentRepository => _customerAppointmentRepository;
        public IBranchRepository BranchRepository => _branchRepository;
        public IApplicationSettingRepository ApplicationSettingRepository => _applicationSettingRepository;
        public IEmployeeLeaveRepository EmployeeLeaveRepository => _employeeLeaveRepository;
        public ITenantRepository TenantRepository => _tenantRepository;
        public IBookingFlowConfigRepository BookingFlowConfigRepository => _bookingFlowConfigRepository;

        public Task BeginTransactionAsync()
        {
            return _repositoryContext.Database.BeginTransactionAsync();
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
