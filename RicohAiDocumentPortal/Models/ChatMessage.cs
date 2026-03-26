namespace RicohAiDocumentPortal.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; } = string.Empty; // "User" or "AI"
        public string Text { get; set; } = string.Empty;
    }
}