using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Patient_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First name")]
        public required string FirstName {  get; set; }
        [Display(Name = "Last name")]
        public required string LastName { get; set; }
        public string Fullname => $"{FirstName} {LastName}";

        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public required DateTime DateOfBirth { get; set; }
        [Display(Name = "Street name and number")]
        public required string StreetNameAndNumber { get; set; }
        public required string City {  get; set; }
        [Display(Name = "Postal code")]
        public required string PostalCode { get; set; }
        public required string Country { get; set; }

        public int EmergencyDetailsId { get; set; }
        public virtual required EmergencyDetails EmergencyDetails { get; set; }
        public int InsuranceDetailsId { get; set; }
        public virtual required InsuranceDetails InsuranceDetails { get; set; }
    }

    public class EmergencyDetails
    {
        public int Id { get; set; }
        [Display(Name = "Emergency name")]
        public required string EmergencyName { get; set; }
        [Display(Name = "Phone number")]
        public required string PhoneNumber { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }

    public class InsuranceDetails
    {
        public int Id { get; set; }
        public required string Provider { get; set; }
        [Display(Name = "Policy number")]
        public required string PolicyNumber { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
