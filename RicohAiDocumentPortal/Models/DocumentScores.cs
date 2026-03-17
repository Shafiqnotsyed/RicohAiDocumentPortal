namespace RicohAiDocumentPortal.Models
{
    public class DocumentScores
    {
        public int OverallScore { get; set; }
        public int StructureScore { get; set; }
        public int CompletenessScore { get; set; }
        public int AccuracyScore { get; set; }
        public int ComplianceScore { get; set; }
        public int RiskScore { get; set; }
    }
}