using DocsPortal.DAL.Context;
using DocsPortal.DAL.Models;

namespace DocsPortal.DAL.Repositories
{
    public class DocumentsDAL : DALObject
    {
        public DocumentsDAL(StorageContext context) : base(context) {}

        public Document? GetDocument(Guid uid)
        {
            return Context.Documents.FirstOrDefault(doc => doc.DocumentGUID == uid);
        }

        public List<Document> GetAllDocuments()
        {
            return Context.Documents.ToList();
        }

        public void AddDocument(Document document)
        {
            Context.Documents.Add(document);
            Context.SaveChanges();
        }

        public void UpdateDocument(Document document)
        {
            var existingDoc = GetDocument(document.DocumentGUID);
            if (existingDoc != null)
            {
                existingDoc.Title = document.Title;
                existingDoc.Content = document.Content;
                existingDoc.Version = document.Version;
                existingDoc.UpdatedAt = DateTime.Now;
                Context.SaveChanges();
            }
        }

        public void DeleteDocument(Guid uid)
        {
            var document = GetDocument(uid);
            if (document != null)
            {
                Context.Documents.Remove(document);
                Context.SaveChanges();
            }
        }
    }
}
