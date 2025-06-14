using DocsPortal.BLL.Context;
using DocsPortal.Library;

namespace DocsPortal.BLL
{
    public class DocumentsBL : BLObject
    {
        public DocumentsBL(BLContext context) : base(context) { }

        public DocumentDTO? GetDocument(Guid uid)
        {
            var document = Context.DALContext.DocumentsDAL.GetDocument(uid);
            if (document == null)
                return null;

            return new DocumentDTO
            {
                DocumentUid = document.DocumentGUID,
                Title = document.Title,
                Content = document.Content,
                Version = document.Version,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt
            };
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
                DocumentGUID = Guid.NewGuid(),
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
            Context.DALContext.DocumentsDAL.UpdateDocument(document);
        }

        public void DeleteDocument(Guid uid)
        {
            Context.DALContext.DocumentsDAL.DeleteDocument(uid);
        }
    }
}
