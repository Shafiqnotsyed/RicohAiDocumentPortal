using Microsoft.AspNetCore.Mvc;

namespace RicohAiDocumentPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}