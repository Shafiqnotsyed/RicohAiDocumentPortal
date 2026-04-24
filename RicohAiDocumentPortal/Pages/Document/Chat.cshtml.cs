using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RicohAiDocumentPortal.Helpers;
using RicohAiDocumentPortal.Models;
using RicohAiDocumentPortal.Services;

namespace RicohAiDocumentPortal.Pages.Document;

public class ChatModel : PageModel
{
    private readonly IDocumentChatService _chatService;
    private readonly GeminiSettings _settings;

    private const string ChatHistoryKey = "ChatHistory";
    private const string ChatDocumentKey = "ChatDocument";

    public ChatModel(IDocumentChatService chatService, IOptions<GeminiSettings> settings)
    {
        _chatService = chatService;
        _settings = settings.Value;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        public string DocumentText { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
    }

    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

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

        // Preloaded from Dashboard "Send to Chat" — starts a fresh conversation
        if (TempData["PreloadedDocument"] is string preloaded && !string.IsNullOrWhiteSpace(preloaded))
        {
            HttpContext.Session.SetString(ChatDocumentKey, preloaded);
            HttpContext.Session.Remove(ChatHistoryKey);
            Input.DocumentText = preloaded;
        }
        else
        {
            Input.DocumentText = HttpContext.Session.GetString(ChatDocumentKey) ?? string.Empty;
        }

        Messages = GetChatHistory();
        return Page();
    }

    public async Task<IActionResult> OnPostAskAsync()
    {
        if (string.IsNullOrWhiteSpace(Input.DocumentText))
        {
            TempData["ErrorMessage"] = "Document text is required.";
            Messages = GetChatHistory();
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Input.Question))
        {
            TempData["ErrorMessage"] = "Please enter a question.";
            Messages = GetChatHistory();
            return Page();
        }

        var history = GetChatHistory();

        history.Add(new ChatMessage { Sender = "User", Text = Input.Question });

        var response = await _chatService.AskAsync(Input.DocumentText, Input.Question);

        history.Add(new ChatMessage
        {
            Sender = "AI",
            Text = response.Answer,
            ModelLabel = response.GeneratedByModel,
            AttemptNumber = response.AttemptNumber,
            UsedFallback = response.UsedFallback
        });

        SaveChatHistory(history);
        HttpContext.Session.SetString(ChatDocumentKey, Input.DocumentText);

        return RedirectToPage();
    }

    public IActionResult OnPostReset()
    {
        HttpContext.Session.Remove(ChatHistoryKey);
        HttpContext.Session.Remove(ChatDocumentKey);
        return RedirectToPage();
    }

    private List<ChatMessage> GetChatHistory()
    {
        var json = HttpContext.Session.GetString(ChatHistoryKey);
        if (string.IsNullOrWhiteSpace(json))
            return new List<ChatMessage>();
        return JsonSerializer.Deserialize<List<ChatMessage>>(json) ?? new List<ChatMessage>();
    }

    private void SaveChatHistory(List<ChatMessage> history)
    {
        HttpContext.Session.SetString(ChatHistoryKey, JsonSerializer.Serialize(history));
    }
}
