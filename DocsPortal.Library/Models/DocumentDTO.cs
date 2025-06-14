namespace DocsPortal.Library
{
    public class DocumentDTO
    {
        public Guid DocumentUid { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
