using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;
using RicohAiDocumentPortal.Models;

namespace RicohAiDocumentPortal.Pages.Document;

public class FeedbackResultModel : PageModel
{
    public AnalyzeDocumentResponse? Result { get; set; }

    public IActionResult OnGet()
    {
        var redirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (redirect is not null) return redirect;

        var json = TempData["FeedbackResult"] as string;
        if (!string.IsNullOrEmpty(json))
        {
            Result = System.Text.Json.JsonSerializer.Deserialize<AnalyzeDocumentResponse>(json,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return Page();
    }
}
