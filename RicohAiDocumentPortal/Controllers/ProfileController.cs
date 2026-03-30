using Microsoft.AspNetCore.Mvc;

namespace RicohAiDocumentPortal.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}