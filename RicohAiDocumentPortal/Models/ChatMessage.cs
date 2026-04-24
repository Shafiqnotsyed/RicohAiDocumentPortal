namespace RicohAiDocumentPortal.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; } = string.Empty; // "User" or "AI"
        public string Text { get; set; } = string.Empty;
        public string ModelLabel { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public bool UsedFallback { get; set; }
    }
}
