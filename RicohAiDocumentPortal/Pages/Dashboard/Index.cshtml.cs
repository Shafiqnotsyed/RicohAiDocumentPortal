using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;

namespace RicohAiDocumentPortal.Pages.Dashboard;

public class IndexModel : PageModel
{
    private const string DashboardDocumentKey = "DashboardDocument";

    [BindProperty]
    public string? SummaryText { get; set; }

    public IActionResult OnGet()
    {
        return AuthHelper.RedirectIfNotAuthenticated(this) ?? Page();
    }

    public IActionResult OnPostStoreDocument()
    {
        if (!string.IsNullOrWhiteSpace(SummaryText))
        {
            HttpContext.Session.SetString(DashboardDocumentKey, SummaryText);
            return new JsonResult(new { success = true });
        }

        return new JsonResult(new { success = false, message = "No document text provided." });
    }

    public IActionResult OnPostSendToChat()
    {
        var stored = HttpContext.Session.GetString(DashboardDocumentKey);

        if (!string.IsNullOrWhiteSpace(stored))
            TempData["PreloadedDocument"] = stored;

        return RedirectToPage("/Document/Chat");
    }

    public IActionResult OnPostSendToFeedback()
    {
        var stored = HttpContext.Session.GetString(DashboardDocumentKey);

        if (!string.IsNullOrWhiteSpace(stored))
            TempData["PreloadedDocument"] = stored;

        return RedirectToPage("/Document/Feedback");
    }
}