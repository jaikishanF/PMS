using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Patient_Management_System.Models
{
    public enum AppointmentType
    {
        Consultation,
        FollowUp,
        Emergency
    }

    public class Booking
    {
        public int ID { get; set; }

        [Required]
        public required AppointmentType AppointmentType { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        // Foreign Key
        public string? PatientId { get; set; }

        // Navigation property for the Patient
        [ForeignKey("PatientId")]
        public virtual ApplicationUser? Patient { get; set; }
    }
}
