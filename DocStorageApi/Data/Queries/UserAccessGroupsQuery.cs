namespace DocStorageApi.Data.Queries
{
    public record UserAccessGroupsQuery(Guid? UserId) : IBaseQuery
    {
        public string Script => @"
                                    SELECT u.id as UserId, 
                                           u.Name as UserName,       
                                           ag.id as AccessGroupId,
                                           ag.Name as AccessGroupName,
                                           ag.status as AccessGroupStatus,
                                           uag.id as UserAccessGroupId,
                                           u.name as GrantedByUserName,
                                           uag.created_at as PermissionGrantedAt

                                    FROM user_access_groups uag  
                                    JOIN users u ON uag.user_id = u.id 
                                    JOIN access_groups ag ON uag.access_group_id = ag.id
                                    WHERE @UserId is null OR u.id = @UserId;
                                    ";
        public object Param => new { UserId };
    }
}
