using Microsoft.AspNetCore.Mvc;
using RicohAiDocumentPortal.Models;
using RicohAiDocumentPortal.Services;

namespace RicohAiDocumentPortal.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentAnalysisService _analysisService;
        private readonly IDocumentChatService _chatService;

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
            return View(new ChatDocumentRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DocumentText))
            {
                TempData["ErrorMessage"] = "Document text is required.";
                return View(request);
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                TempData["ErrorMessage"] = "Please enter a question.";
                return View(request);
            }

            var response = await _chatService.AskAsync(
                request.DocumentText,
                request.Question);

            return View("ChatResult", response);
        }
    }
}