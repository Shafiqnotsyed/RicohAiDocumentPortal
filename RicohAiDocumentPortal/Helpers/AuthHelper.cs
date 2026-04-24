using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RicohAiDocumentPortal.Helpers;

public static class AuthHelper
{
    public static IActionResult? RedirectIfNotAuthenticated(PageModel page)
    {
        if (page.HttpContext.Session.GetString("IsAuthenticated") != "true")
        {
            return page.RedirectToPage("/Account/Login");
        }
        return null;
    }
}
