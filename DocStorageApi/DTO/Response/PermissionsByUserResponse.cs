using Amazon.S3.Model;

namespace DocStorageApi.DTO.Response
{
    public class PermissionsByUserResponse
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public IEnumerable<DocumentsWithAccessMethodResponse> DocumentsAvailable { get; set; }

    }

    public class DocumentsWithAccessMethodResponse
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public IEnumerable<string> AccessGroupNames { get; set; }
    }
}
