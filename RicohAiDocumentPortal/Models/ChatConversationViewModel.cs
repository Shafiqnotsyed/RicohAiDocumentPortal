using System.Collections.Generic;

namespace RicohAiDocumentPortal.Models
{
    public class ChatConversationViewModel
    {
        public string DocumentText { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public List<ChatMessage> Messages { get; set; } = new();
    }
}