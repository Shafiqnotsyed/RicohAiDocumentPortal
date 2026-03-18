namespace RicohAiDocumentPortal.Models
{
    public class AnalyzeDocumentRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string DocumentText { get; set; } = string.Empty;
    }
}