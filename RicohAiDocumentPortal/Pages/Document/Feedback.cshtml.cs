using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;
using RicohAiDocumentPortal.Models;
using RicohAiDocumentPortal.Services;

namespace RicohAiDocumentPortal.Pages.Document;

public class FeedbackModel : PageModel
{
    private readonly IDocumentAnalysisService _analysisService;
    private readonly GeminiSettings _settings;

    public FeedbackModel(IDocumentAnalysisService analysisService, IOptions<GeminiSettings> settings)
    {
        _analysisService = analysisService;
        _settings = settings.Value;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        public string FileName { get; set; } = string.Empty;
        public string DocumentText { get; set; } = string.Empty;
    }

    public AnalyzeDocumentResponse? Result { get; set; }

    public List<string> LoadingModels => new List<string>
    {
        _settings.ModelName,
        _settings.FallbackModelName,
        _settings.SecondFallbackModelName
    }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


    public IActionResult OnGet()
    {
        var redirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (redirect is not null) return redirect;

        // Preloaded from Dashboard "Send to Feedback" button
        if (TempData["PreloadedDocument"] is string preloaded && !string.IsNullOrWhiteSpace(preloaded))
        {
            Input.DocumentText = preloaded;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = AuthHelper.RedirectIfNotAuthenticated(this);
        if (redirect is not null) return redirect;

        if (string.IsNullOrWhiteSpace(Input.DocumentText))
        {
            TempData["ErrorMessage"] = "Document text is required.";
            return Page();
        }

        Result = await _analysisService.AnalyzeAsync(Input.FileName, Input.DocumentText);

        TempData["FeedbackResult"] = System.Text.Json.JsonSerializer.Serialize(Result);
        return RedirectToPage("/Document/FeedbackResult");
    }
}
