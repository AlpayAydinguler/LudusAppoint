using System.ComponentModel.DataAnnotations;

namespace LudusAppoint.Models.Enums
{
    public enum Permissions
    {
        [Display(Name = "Index Age Group", Description = "Have access to Index in Age Group")]
        AgeGroup_Index,

        [Display(Name = "Create Age Group", Description = "Have access to Create in Age Group")]
        AgeGroup_Create,

        [Display(Name = "Update Age Group", Description = "Have access to Update in Age Group")]
        AgeGroup_Update,

        [Display(Name = "Delete Age Group", Description = "Have access to Delete in Age Group")]
        AgeGroup_Delete,

        [Display(Name = "Index Application Setting", Description = "Have access to Index in Application Setting")]
        ApplicationSetting_Index,

        [Display(Name = "Create Application Setting", Description = "Have access to Create in Application Setting")]
        ApplicationSetting_Create,

        [Display(Name = "Update Application Setting", Description = "Have access to Update in Application Setting")]
        ApplicationSetting_Update,

        [Display(Name = "Delete Application Setting", Description = "Have access to Delete in Application Setting")]
        ApplicationSetting_Delete,

        [Display(Name = "Index Branch", Description = "Have access to Index in Branch")]
        Branch_Index,

        [Display(Name = "Create Branch", Description = "Have access to Create in Branch")]
        Branch_Create,

        [Display(Name = "Update Branch", Description = "Have access to Update in Branch")]
        Branch_Update,

        [Display(Name = "Delete Branch", Description = "Have access to Delete in Branch")]
        Branch_Delete,

        [Display(Name = "Index Customer Appointment", Description = "Have access to Index in Customer Appointment")]
        CustomerAppointment_Index,

        [Display(Name = "PendingConfirmationList Customer Appointment", Description = "Have access to PendingConfirmationList in Customer Appointment")]
        CustomerAppointment_PendingConfirmationList,

        [Display(Name = "Create Customer Appointment", Description = "Have access to Create in Customer Appointment")]
        CustomerAppointment_Create,

        [Display(Name = "Update Customer Appointment", Description = "Have access to Update in Customer Appointment")]
        CustomerAppointment_Update,

        [Display(Name = "ApproveAppointment Customer Appointment", Description = "Have access to ApproveAppointment in Customer Appointment")]
        CustomerAppointment_ApproveAppointment,

        [Display(Name = "CancelAppointment Customer Appointment", Description = "Have access to CancelAppointment in Customer Appointment")]
        CustomerAppointment_CancelAppointment,

        [Display(Name = "GetOfferedServices Customer Appointment", Description = "Have access to GetOfferedServices in Customer Appointment")]
        CustomerAppointment_GetOfferedServices,

        [Display(Name = "GetEmployees Customer Appointment", Description = "Have access to GetEmployees in Customer Appointment")]
        CustomerAppointment_GetEmployees,

        [Display(Name = "GetReservedDaysTimes Customer Appointment", Description = "Have access to GetReservedDaysTimes in Customer Appointment")]
        CustomerAppointment_GetReservedDaysTimes,

        [Display(Name = "Index Dashboard", Description = "Have access to Index in Dashboard")]
        Dashboard_Index,

        [Display(Name = "Index Employee", Description = "Have access to Index in Employee")]
        Employee_Index,

        [Display(Name = "Update Employee", Description = "Have access to Update in Employee")]
        Employee_Update,

        [Display(Name = "Create Employee", Description = "Have access to Create in Employee")]
        Employee_Create,

        [Display(Name = "Delete Employee", Description = "Have access to Delete in Employee")]
        Employee_Delete,

        [Display(Name = "Index Employee Leave", Description = "Have access to Index in Employee Leave")]
        EmployeeLeave_Index,

        [Display(Name = "Create Employee Leave", Description = "Have access to Create in Employee Leave")]
        EmployeeLeave_Create,

        [Display(Name = "Update Employee Leave", Description = "Have access to Update in Employee Leave")]
        EmployeeLeave_Update,

        [Display(Name = "Delete Employee Leave", Description = "Have access to Delete in Employee Leave")]
        EmployeeLeave_Delete,

        [Display(Name = "Index Offered Service", Description = "Have access to Index in Offered Service")]
        OfferedService_Index,

        [Display(Name = "Create Offered Service", Description = "Have access to Create in Offered Service")]
        OfferedService_Create,

        [Display(Name = "Update Offered Service", Description = "Have access to Update in Offered Service")]
        OfferedService_Update,

        [Display(Name = "Delete Offered Service", Description = "Have access to Delete in Offered Service")]
        OfferedService_Delete,

        [Display(Name = "Index Role", Description = "Have access to Index in Role")]
        Role_Index,

        [Display(Name = "Create Role", Description = "Have access to Create in Role")]
        Role_Create,

        [Display(Name = "Update Role", Description = "Have access to Update in Role")]
        Role_Update,

        [Display(Name = "Delete Role", Description = "Have access to Delete in Role")]
        Role_Delete,

        [Display(Name = "Index User", Description = "Have access to Index in User")]
        User_Index,

        [Display(Name = "Create User", Description = "Have access to Create in User")]
        User_Create,

        [Display(Name = "Update User", Description = "Have access to Update in User")]
        User_Update,

        [Display(Name = "Delete User", Description = "Have access to Delete in User")]
        User_Delete
    }
}
