namespace DocStorageApi.DTO.Response
{
    public class DocumentAccessGroupsResponse
    {
        public int DocumentId { get; set; }

        public string DocumentName { get; set; }

        public string GrantedByUserName { get; set; }

        public string AccessGroupName { get; set; }
    }
}
