using Microsoft.AspNetCore.Mvc;

namespace RicohAiDocumentPortal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}