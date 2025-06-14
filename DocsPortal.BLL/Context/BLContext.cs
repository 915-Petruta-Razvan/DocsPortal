using DocsPortal.DAL.Context;
using Microsoft.Extensions.Caching.Distributed;

namespace DocsPortal.BLL.Context
{
    public class BLContext
    {
        public DALContext DALContext { get; }
        public IDistributedCache Cache { get; set; }
        public DocumentsBL DocumentsBL { get; }

        public BLContext(DALContext dalContext, IDistributedCache cache)
        {
            Cache = cache;
            DALContext = dalContext;
            DocumentsBL = new DocumentsBL(this);
        }
    }
}
