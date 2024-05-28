using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Patient_Management_System.Data;
using Patient_Management_System.Models;
using System.Diagnostics.Metrics;

namespace Patient_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly PMSContext context;

        public AdminController(UserManager<ApplicationUser> userManager, PMSContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<ApplicationUser> GetPatientById(string id)
        {
            var patient = await context.Users
                                        .Include(u => u.InsuranceDetails)
                                        .Include(u => u.EmergencyDetails)
                                        .FirstOrDefaultAsync(u => u.Id == id);

            return patient;
        }

        public async Task<IActionResult> GetPatient(string lastName)
        {
            var patients = await context.Users
                                        .Where(u => u.LastName == lastName)
                                        .Include(u => u.InsuranceDetails)
                                        .Include(u => u.EmergencyDetails)
                                        .ToListAsync();

            if (patients == null || !patients.Any())
            {
                ViewBag.error = "error";
            }

            return View(patients);
        }

        public async Task<IActionResult> Patients()
        {
            var patients = await context.Users
                                        .OrderBy(u => u.FirstName)
                                        .ThenBy(u => u.LastName)
                                        .ToListAsync();

            if (patients == null || patients.Count == 0)
            {
                ViewBag.error = "error";
            }

            return View(patients);
        }

        public async Task<IActionResult> PatientDetails(string id)
        {
            var patient = await GetPatientById(id);

            if (patient == null)
            {
                ViewBag.error = "error";
            }
            return View(patient);
        }

        public async Task<IActionResult> PatientEdit(string id)
        {
            var patient = await GetPatientById(id);

            if (patient == null)
            {
                ViewBag.error = "error";
            }

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> PatientEdit(ApplicationUser model)
        {
            var patient = await GetPatientById(model.Id);
            if (ModelState.IsValid)
            {
                patient.UserName = model.Email;
                patient.Email = model.Email;
                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.DateOfBirth = model.DateOfBirth;
                patient.PhoneNumber = model.PhoneNumber;
                patient.StreetNameAndNumber = model.StreetNameAndNumber;
                patient.City = model.City;
                patient.PostalCode = model.PostalCode;
                patient.Country = model.Country;
                patient.EmergencyDetails.EmergencyName = model.EmergencyDetails.EmergencyName;
                patient.EmergencyDetails.PhoneNumber = model.EmergencyDetails.PhoneNumber;
                patient.InsuranceDetails.Provider = model.InsuranceDetails.Provider;
                patient.InsuranceDetails.PolicyNumber = model.InsuranceDetails.PolicyNumber;

                await context.SaveChangesAsync();
                return RedirectToAction("PatientDetails", new { id = model.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> PatientConfirmDelete(string id)
        {
            var patient = await GetPatientById(id);
            if (patient == null)
            {
                ViewBag.error = "error";
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientDeleteConfirmed(string id)
        {
            var patient = await GetPatientById(id);
            if (patient != null)
            {
                context.Users.Remove(patient);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Patient successfully deleted.";
                return RedirectToAction("Patients");
            }
            TempData["ErrorMessage"] = "Patient not found.";
            return RedirectToAction("Patients");
        }

    }
}
