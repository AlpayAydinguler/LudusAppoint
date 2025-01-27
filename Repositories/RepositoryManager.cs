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
        //private readonly IShopSettingsRepository _shopSettingsRepository;
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;

        public RepositoryManager(IAgeGroupRepository ageGroupRepository,
                                 RepositoryContext repositoryContext,
                                 IOfferedServiceRepository offeredServiceRepository,
                                 IEmployeeRepository employeeRepository,
                                 ICustomerAppointmentRepository customerAppointmentRepository,
                                 IBranchRepository branchRepository,
                                 //IShopSettingsRepository shopSettingsRepository,
                                 IEmployeeLeaveRepository employeeLeaveRepository)
        {
            _ageGroupRepository = ageGroupRepository;
            _repositoryContext = repositoryContext;
            _offeredServiceRepository = offeredServiceRepository;
            _employeeRepository = employeeRepository;
            _customerAppointmentRepository = customerAppointmentRepository;
            _branchRepository = branchRepository;
            //_shopSettingsRepository = shopSettingsRepository;
            _employeeLeaveRepository = employeeLeaveRepository;
        }

        public IAgeGroupRepository AgeGroupRepository => _ageGroupRepository;

        public IOfferedServiceRepository OfferedServiceRepository => _offeredServiceRepository;
        public IEmployeeRepository EmployeeRepository => _employeeRepository;
        public ICustomerAppointmentRepository CustomerAppointmentRepository => _customerAppointmentRepository;
        public IBranchRepository BranchRepository => _branchRepository;
        //public IShopSettingsRepository ShopSettingsRepository => _shopSettingsRepository;
        public IEmployeeLeaveRepository EmployeeLeaveRepository => _employeeLeaveRepository;

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
