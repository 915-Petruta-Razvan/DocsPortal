using DocsPortal.DAL.Repositories;

namespace DocsPortal.DAL.Context
{
    public class DALContext
    {
        public StorageContext Context { get; }
        public DocumentsDAL DocumentsDAL { get; }

        public DALContext(StorageContext context)
        {
            Context = context;
            DocumentsDAL = new DocumentsDAL(context);
        }
    }
}
