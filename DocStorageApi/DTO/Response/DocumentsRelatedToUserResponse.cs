namespace DocStorageApi.DTO.Response
{
    public class DocumentsRelatedToUserResponse
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public DateTime GrantedAt { get; set; }

        public Guid DocumentId { get; set; }

        public string DocumentName { get; set; }

        public string DocumentDescription { get; set; }
    }
}
