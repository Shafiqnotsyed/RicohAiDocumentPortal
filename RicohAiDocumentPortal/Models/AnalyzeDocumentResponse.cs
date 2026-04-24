namespace RicohAiDocumentPortal.Models
{
    public class AnalyzeDocumentResponse
    {
        public string DocumentType { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public Dictionary<string, string> ExtractedFields { get; set; } = new();
        public DocumentScores Scores { get; set; } = new();
        public string Grade { get; set; } = string.Empty;
        public List<string> RedFlags { get; set; } = new();
        public List<string> Corrections { get; set; } = new();
        public double ConfidenceScore { get; set; }
        public string GeneratedByModel { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public bool UsedFallback { get; set; }
        public string GenerationStatus { get; set; } = string.Empty;
    }
}
