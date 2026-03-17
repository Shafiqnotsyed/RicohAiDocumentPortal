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
        public async Task<IActionResult> Feedback()
        {
            string sampleText = @"
Invoice Number: INV-1001
Supplier: ABC Supplies
Invoice Date: 2026-03-13
Total Amount: R 15,400.00
Due Date: 2026-03-20
Payment Terms: Net 7
";

            var model = await _analysisService.AnalyzeAsync("sample.pdf", sampleText);
            return View(model);
        }

        [HttpGet]
        public IActionResult Chat()
        {
            return View(new ChatDocumentResponse());
        }

        [HttpPost]
        public async Task<IActionResult> Chat(string question)
        {
            string sampleText = @"
Invoice Number: INV-1001
Supplier: ABC Supplies
Invoice Date: 2026-03-13
Total Amount: R 15,400.00
Due Date: 2026-03-20
Payment Terms: Net 7
";

            var response = await _chatService.AskAsync(sampleText, question ?? string.Empty);
            return View(response);
        }
    }
}