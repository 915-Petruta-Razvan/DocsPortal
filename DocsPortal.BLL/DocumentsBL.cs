using DocsPortal.BLL.Context;
using DocsPortal.Library;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DocsPortal.BLL
{
    public class DocumentsBL : BLObject
    {
        public DocumentsBL(BLContext context) : base(context) { }

        public DocumentDTO? GetDocument(Guid uid)
        {
            string cacheKey = GetCacheKey("document", uid);
            string? cachedDocument = GetRedisValue(cacheKey);
            if (!string.IsNullOrEmpty(cachedDocument))
                return JsonSerializer.Deserialize<DocumentDTO>(cachedDocument);

            var document = Context.DALContext.DocumentsDAL.GetDocument(uid);
            if (document == null)
                return null;

            var documentDto = new DocumentDTO
            {
                DocumentUid = document.DocumentGUID,
                Title = document.Title,
                Content = document.Content,
                Version = document.Version,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt
            };

            SetRedisValue(cacheKey, JsonSerializer.Serialize(documentDto));

            return documentDto;
        }

        public List<DocumentDTO> GetAllDocuments()
        {
            var documents = Context.DALContext.DocumentsDAL.GetAllDocuments();

            return documents.Select(doc => new DocumentDTO
            {
                DocumentUid = doc.DocumentGUID,
                Title = doc.Title,
                Content = doc.Content,
                Version = doc.Version,
                CreatedAt = doc.CreatedAt,
                UpdatedAt = doc.UpdatedAt
            }).ToList();
        }

        public void AddDocument(DocumentDTO documentDto)
        {
            var document = new DAL.Models.Document
            {
                DocumentGUID = documentDto.DocumentUid,
                Title = documentDto.Title,
                Content = documentDto.Content,
                Version = 1, // Initial version
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            Context.DALContext.DocumentsDAL.AddDocument(document);
        }

        public void UpdateDocument(DocumentDTO documentDto)
        {
            var document = new DAL.Models.Document
            {
                DocumentGUID = documentDto.DocumentUid,
                Title = documentDto.Title,
                Content = documentDto.Content,
                Version = documentDto.Version + 1, // Increment version
                UpdatedAt = DateTime.Now
            };

            Context.Cache.Remove(GetCacheKey("document", document.DocumentGUID));
            Context.DALContext.DocumentsDAL.UpdateDocument(document);
        }

        public void DeleteDocument(Guid uid)
        {
            Context.Cache.Remove(GetCacheKey("document", uid));
            Context.DALContext.DocumentsDAL.DeleteDocument(uid);
        }
    }
}
