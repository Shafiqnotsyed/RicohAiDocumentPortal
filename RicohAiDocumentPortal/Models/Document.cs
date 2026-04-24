using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RicohAiDocumentPortal.Models
{
    [Table("documents")]
    public class Document : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
