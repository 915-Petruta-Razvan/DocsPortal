using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocsPortal.DAL.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Document
    {
        [Key]
        public Guid DocumentGUID { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
