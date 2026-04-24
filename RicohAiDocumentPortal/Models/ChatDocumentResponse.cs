namespace RicohAiDocumentPortal.Models
{
    public class ChatDocumentResponse
    {
        public string Answer { get; set; } = string.Empty;
        public string GeneratedByModel { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public bool UsedFallback { get; set; }
        public string GenerationStatus { get; set; } = string.Empty;
    }
}
