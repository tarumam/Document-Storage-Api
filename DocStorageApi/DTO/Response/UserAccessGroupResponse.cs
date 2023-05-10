namespace DocStorageApi.DTO.Response
{
    public class UserAccessGroupResponse
    {
        public Guid AccessGroupId { get; set; }
        public string AccessGroupName { get; set; }
        public string AccessGroupStatus { get; set; }
        public Guid UserAccessGroupId { get; set; }
        public string GrantedByUserName { get; set; }
        public DateTime PermissionGrantedAt { get; set; }
    }
}
