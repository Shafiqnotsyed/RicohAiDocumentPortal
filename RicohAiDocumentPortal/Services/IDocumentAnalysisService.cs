using RicohAiDocumentPortal.Models;

namespace RicohAiDocumentPortal.Services
{
    public interface IDocumentAnalysisService
    {
        Task<AnalyzeDocumentResponse> AnalyzeAsync(string fileName, string documentText);
    }
}