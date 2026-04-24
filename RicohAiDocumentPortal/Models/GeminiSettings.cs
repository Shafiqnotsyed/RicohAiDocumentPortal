namespace RicohAiDocumentPortal.Models
{
    public class GeminiSettings
    {
        public string ModelName { get; set; } = "gemini-2.5-flash";
        public string FallbackModelName { get; set; } = "gemini-2.5-flash-lite";
        public string SecondFallbackModelName { get; set; } = "gemini-1.5-flash";
    }
}