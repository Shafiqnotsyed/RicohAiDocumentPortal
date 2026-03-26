using Microsoft.AspNetCore.Mvc;
using RicohAiDocumentPortal.Models;
using RicohAiDocumentPortal.Services;
using System.Text.Json;

namespace RicohAiDocumentPortal.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentAnalysisService _analysisService;
        private readonly IDocumentChatService _chatService;

        private const string ChatHistoryKey = "ChatHistory";
        private const string ChatDocumentKey = "ChatDocument";

        public DocumentController(
            IDocumentAnalysisService analysisService,
            IDocumentChatService chatService)
        {
            _analysisService = analysisService;
            _chatService = chatService;
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            return View(new AnalyzeDocumentRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Feedback(AnalyzeDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DocumentText))
            {
                TempData["ErrorMessage"] = "Document text is required.";
                return View(request);
            }

            var result = await _analysisService.AnalyzeAsync(
                request.FileName,
                request.DocumentText);

            return View("FeedbackResult", result);
        }

        [HttpGet]
        public IActionResult Chat()
        {
            var model = new ChatConversationViewModel
            {
                DocumentText = HttpContext.Session.GetString(ChatDocumentKey) ?? string.Empty,
                Messages = GetChatHistory()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatConversationViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.DocumentText))
            {
                TempData["ErrorMessage"] = "Document text is required.";
                request.Messages = GetChatHistory();
                return View(request);
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                TempData["ErrorMessage"] = "Please enter a question.";
                request.Messages = GetChatHistory();
                return View(request);
            }

            var history = GetChatHistory();

            history.Add(new ChatMessage
            {
                Sender = "User",
                Text = request.Question
            });

            var response = await _chatService.AskAsync(
                request.DocumentText,
                request.Question);

            history.Add(new ChatMessage
            {
                Sender = "AI",
                Text = response.Answer
            });

            SaveChatHistory(history);
            HttpContext.Session.SetString(ChatDocumentKey, request.DocumentText);

            return RedirectToAction("Chat");
        }

        [HttpPost]
        public IActionResult ResetChat()
        {
            HttpContext.Session.Remove(ChatHistoryKey);
            HttpContext.Session.Remove(ChatDocumentKey);
            return RedirectToAction("Chat");
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
            var json = JsonSerializer.Serialize(history);
            HttpContext.Session.SetString(ChatHistoryKey, json);
        }
    }
}