namespace DocStorageApi.Data.Queries
{
    /// <summary>
    /// Query that select a user by its Id
    /// </summary>
    /// <param name="UserId">UserId</param>
    public record GetUserByIdQuery(Guid UserId) : IBaseQuery
    {
        public string Script => @"SELECT 
                                    id as UserId,
                                    name as Name, 
                                    password as Password,
                                    token_id as TokenId,
                                    status as IsActive,
                                    role as Role,
                                    created_at as CreatedAt,
                                    updated_at as UpdatedAt
                                FROM 
                                    users u
                                 WHERE id = @UserId;";
        public object Param => new { UserId };
    }
}
