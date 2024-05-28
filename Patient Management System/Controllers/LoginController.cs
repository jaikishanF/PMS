using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patient_Management_System.Models;

namespace Patient_Management_System.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    StreetNameAndNumber = model.StreetNameAndNumber,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    EmergencyDetails = new EmergencyDetails
                    {
                        EmergencyName = model.EmergencyDetails.EmergencyName,
                        PhoneNumber = model.EmergencyDetails.PhoneNumber
                    },
                    InsuranceDetails = new InsuranceDetails
                    {
                        Provider = model.InsuranceDetails.Provider,
                        PolicyNumber = model.InsuranceDetails.PolicyNumber
                    }
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("patients", "admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
