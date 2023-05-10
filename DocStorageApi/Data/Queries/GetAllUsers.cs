namespace DocStorageApi.Data.Queries
{
    /// <summary>
    /// Query that selects all users from the database
    /// </summary>
    public record ListAllUsersQuery : IBaseQuery
    {
        public string Script => @"SELECT 
                                    id as UserId,
                                    name as Name, 
                                    role as Role,
                                    password as Password,
                                    token_id as TokenId,
                                    status as IsActive,
                                    created_at as CreatedAt,
                                    updated_at as UpdatedAt
                                FROM 
                                    users u";
        public object Param => null!;
    }
}
