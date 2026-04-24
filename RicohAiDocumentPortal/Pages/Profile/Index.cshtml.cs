using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;

namespace RicohAiDocumentPortal.Pages.Profile;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private const string ThemeSessionKey = "ThemeMode";

    public string CurrentTheme { get; set; } = "dark";
    public string UserEmail { get; set; } = "user@ricohportal.com";

    public IActionResult OnGet()
    {
        var authRedirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (authRedirect != null)
        {
            return authRedirect;
        }

        CurrentTheme = HttpContext.Session.GetString(ThemeSessionKey) ?? "dark";
        UserEmail = HttpContext.Session.GetString("UserEmail") ?? "user@ricohportal.com";
        return Page();
    }

    public IActionResult OnPostSetTheme([FromBody] ThemeRequest request)
    {
        var authRedirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (authRedirect != null)
        {
            return new JsonResult(new
            {
                success = false,
                message = "You must be logged in to change the theme."
            })
            {
                StatusCode = 401
            };
        }

        if (request == null || string.IsNullOrWhiteSpace(request.Theme))
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                message = "Theme is required."
            });
        }

        var theme = request.Theme.Trim().ToLowerInvariant();
        if (theme != "light" && theme != "dark")
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                message = "Invalid theme."
            });
        }

        HttpContext.Session.SetString(ThemeSessionKey, theme);
        CurrentTheme = theme;

        return new JsonResult(new
        {
            success = true,
            theme
        });
    }

    public class ThemeRequest
    {
        public string Theme { get; set; } = string.Empty;
    }
}
