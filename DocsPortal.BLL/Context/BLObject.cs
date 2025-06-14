namespace DocsPortal.BLL.Context
{
    public class BLObject
    {
        protected BLContext Context;

        public BLObject(BLContext context)
        {
            this.Context = context;
        }
    }
}
