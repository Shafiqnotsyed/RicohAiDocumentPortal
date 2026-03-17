using RicohAiDocumentPortal.Models;

namespace RicohAiDocumentPortal.Services
{
    public interface IDocumentChatService
    {
        Task<ChatDocumentResponse> AskAsync(string documentText, string question);
    }
}