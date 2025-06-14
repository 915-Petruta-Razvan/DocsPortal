namespace DocsPortal.DAL.Context
{
    public class DALObject
    {
        protected StorageContext Context;

        public DALObject(StorageContext context)
        {
            this.Context = context;
        }
    }
}
