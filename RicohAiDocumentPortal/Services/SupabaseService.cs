//using Supabase.Postgrest.Constants;
using RicohAiDocumentPortal.Models;
using static Supabase.Postgrest.Constants;

namespace RicohAiDocumentPortal.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;
        private readonly ILogger<SupabaseService> _logger;

        public SupabaseService(Supabase.Client client, ILogger<SupabaseService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<Document>> GetAllAsync()
        {
            try
            {
                var result = await _client.From<Document>()
                    .Order("created_at", Ordering.Descending)
                    .Get();
                return result.Models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch documents");
                return new List<Document>();
            }
        }
       


        public async Task<Document?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _client.From<Document>()
                    .Where(d => d.Id == id)
                    .Single();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch document {Id}", id);
                return null;
            }
        }

        public async Task<bool> CreateAsync(Document document)
        {
            try
            {
                await _client.From<Document>().Insert(document);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create document");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Document document)
        {
            try
            {
                await _client.From<Document>().Update(document);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update document {Id}", document.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                await _client.From<Document>()
                    .Where(d => d.Id == id)
                    .Delete();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete document {Id}", id);
                return false;
            }
        }
        public async Task<string?> UploadPdfAsync(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, bytes.Length);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";

                await _client.Storage
                    .From("documents")
                    .Upload(bytes, fileName, new Supabase.Storage.FileOptions
                    {
                        ContentType = "application/pdf",
                        Upsert = false
                    });

                // Return the public URL
                var url = _client.Storage
                    .From("documents")
                    .GetPublicUrl(fileName);

                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload PDF");
                return null;
            }
        }
    }
}