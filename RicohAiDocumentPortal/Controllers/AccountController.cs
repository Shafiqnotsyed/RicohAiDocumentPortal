using Microsoft.AspNetCore.Mvc;
using RicohAiDocumentPortal.ViewModels;

namespace RicohAiDocumentPortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Simple test login
            if (model.Email == "admin@ricoh.com" && model.Password == "Password123")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        // GET: Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Normally you would save the user to a database here

            return RedirectToAction("Login");
        }
    }
}