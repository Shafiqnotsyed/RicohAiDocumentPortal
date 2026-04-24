using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RicohAiDocumentPortal.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") != "true")
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
}
