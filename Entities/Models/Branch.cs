namespace Entities.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public String BranchName { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String? District { get; set; }
        public String? Neighbourhood { get; set; }
        public String Street { get; set; }
        public String? Address { get; set; }
        public String? BranchPhoneNumber { get; set; }
        public String? BranchEmail { get; set; }
        public int ReservationInAdvanceDayLimit { get; set; } = 60;
        public bool Status { get; set; } = true;

        //Navigation Properties
        public ICollection<Employee>? Employee { get; set; }
        public ICollection<CustomerAppointment>? CustomerAppointment { get; set; }
        public ShopSettings? ShopSettings { get; set; }
    }
}
