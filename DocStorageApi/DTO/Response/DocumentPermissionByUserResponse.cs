namespace DocStorageApi.DTO.Response
{
    public class DocumentPermissionsWithUserResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string AccessType { get; set; }
        public string AccessGroupName { get; set; }
        public Guid AccessGroupId { get; set; } 
        public Guid DocumentId { get; set; }
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
