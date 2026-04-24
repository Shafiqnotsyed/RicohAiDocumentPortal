using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RicohAiDocumentPortal.Pages.Account;

public class LogoutModel : PageModel
{
    public void OnGet()
    {
        HttpContext.Session.Clear();
    }
}
