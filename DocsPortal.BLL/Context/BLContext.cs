using DocsPortal.DAL.Context;

namespace DocsPortal.BLL.Context
{
    public class BLContext
    {
        public DALContext DALContext { get; }

        public DocumentsBL DocumentsBL { get; }

        public BLContext(DALContext dalContext)
        {
            DALContext = dalContext;
            DocumentsBL = new DocumentsBL(this);
        }
    }
}
