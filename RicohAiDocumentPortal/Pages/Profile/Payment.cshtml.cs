using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;

namespace RicohAiDocumentPortal.Pages.Profile;

public class PaymentModel : PageModel
{
    public IActionResult OnGet()
    {
        var authRedirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (authRedirect != null)
        {
            return authRedirect;
        }

        return Page();
    }
}
