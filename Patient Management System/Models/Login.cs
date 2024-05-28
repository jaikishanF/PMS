using System.ComponentModel.DataAnnotations;

namespace Patient_Management_System.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class Register
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First name")]
        public required string FirstName {  get; set; }

        [Required]
        [Display(Name = "Last name")]
        public required string LastName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Date of birth")]
        public required DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public required string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Street name and number")]
        public required string StreetNameAndNumber { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        public required string PostalCode { get; set; }

        [Required]
        public required string Country { get; set; }

        [Required]
        public required EmergencyDetails EmergencyDetails { get; set; }

        [Required]
        public required InsuranceDetails InsuranceDetails { get; set; }
    }
}
