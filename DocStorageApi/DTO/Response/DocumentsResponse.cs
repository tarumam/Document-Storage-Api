namespace DocStorageApi.DTO.Response
{
    public class DocumentsResponse
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PostedAt { get; set; }
        public bool Status { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
