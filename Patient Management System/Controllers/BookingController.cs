using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Patient_Management_System.Models;

namespace Patient_Management_System.Controllers
{
    public class BookingController : Controller
    {
        private readonly PMSContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingController(PMSContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private async Task PrepareIndexViewData(Booking booking)
        {
            ViewBag.AppointmentTypes = Enum.GetValues(typeof(AppointmentType))
                                       .Cast<AppointmentType>()
                                       .Select(a => new SelectListItem
                                       {
                                           Value = ((int)a).ToString(),
                                           Text = a.ToString(),
                                           Selected = a == booking.AppointmentType
                                       });

            var patients = await context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToListAsync();

            ViewBag.Patients = patients.Select(u => new SelectListItem
                                        {
                                            Value = u.Id,
                                            Text = u.Fullname
                                        }).ToList();

            // Handle case where there are no patients to select
            if (!patients.Any())
            {
                ModelState.AddModelError("", "No patients available to book.");
            }
        }

        public async Task<IActionResult> Index(Booking booking = null)
        {
            if (booking == null)
            {
                var defaultPatient = await context.Users.FirstOrDefaultAsync();
                booking = new Booking
                {
                    BookingDate = DateTime.Today,
                    StartTime = TimeSpan.FromHours(DateTime.Now.Hour),
                    EndTime = TimeSpan.FromHours(DateTime.Now.Hour + 1),
                    AppointmentType = AppointmentType.Consultation,
                    PatientId = "4beb0549-f8be-4166-96c8-390619fb76c4",
                    Patient = await context.Users.FindAsync(booking.PatientId)
                };
            }

            await PrepareIndexViewData(booking);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            booking.Patient = context.Users.Find(booking.PatientId);
            var availability = await context.BookingAvailability
                                    .FirstOrDefaultAsync(ba => ba.Date.Date == booking.BookingDate.Date &&
                                                               ba.StartTime == booking.StartTime &&
                                                               ba.EndTime == booking.EndTime &&
                                                               ba.IsAvailable);

            if (availability == null)
            {
                ModelState.AddModelError("", "No available slots match your booking request.");
                await PrepareIndexViewData(booking);
                return View("Index", booking);
            }

            if (ModelState.IsValid)
            {
                availability.IsAvailable = false;
                context.Update(availability);

                context.Add(booking);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking successfully created.";
                return RedirectToAction(nameof(Index));
            }

            // Reload the necessary data for the view if the model state is not valid
            await PrepareIndexViewData(booking);
            return View("Index", booking); // Use the same view for displaying the form
        }

        public async Task<IActionResult> GetAvailableSlots()
        {
            var availability = await context.BookingAvailability
                                            .Where(a => a.IsAvailable)
                                            .Select(a => new
                                            {
                                                a.Date,
                                                a.StartTime,
                                                a.EndTime
                                            })
                                            .ToListAsync();

            return Json(availability);
        }

        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await context.Booking
                            .OrderByDescending(b => b.BookingDate)
                            .ThenByDescending(b => b.StartTime)
                            .ThenBy(b => b.Patient.LastName)
                            .ThenBy(b => b.Patient.FirstName)
                            .Include(b => b.Patient)
                            .ToListAsync();

            if (bookings ==  null || bookings.Count == 0)
            {
                ViewBag.error = "error";
            }

            return View(bookings);
        }

        //[Route("Booking/GetPatientBookings/{lastName}")]
        public async Task<IActionResult> GetPatientBookings(string lastName)
        {
            var bookings = await context.Booking
                            .Where(b => b.Patient.LastName == lastName)
                            .OrderByDescending(b => b.BookingDate)
                            .ThenByDescending(b => b.StartTime)
                            .ThenBy(b => b.Patient.LastName)
                            .ThenBy(b => b.Patient.FirstName)
                            .Include(b => b.Patient)
                            .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                ViewBag.Error = "No bookings found for the specified last name.";
            }

            return View(bookings);
        }
    }
}