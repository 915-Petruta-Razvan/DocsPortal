using DocsPortal.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DocsPortal.DAL.Context
{
    public class StorageContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public StorageContext(DbContextOptions<StorageContext> options) : base(options) {}
    }
}
