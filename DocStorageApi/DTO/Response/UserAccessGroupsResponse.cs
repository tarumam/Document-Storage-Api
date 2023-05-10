namespace DocStorageApi.DTO.Response
{
    public class UserAccessGroupsResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<UserAccessGroupResponse> AccessGroups { get; set; } = new List<UserAccessGroupResponse>();
    }
}
