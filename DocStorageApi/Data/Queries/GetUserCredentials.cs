namespace DocStorageApi.Data.Queries
{
    //TODO: Read from sql file

    /// <summary>
    /// Query that selectcs users and related access groups
    /// </summary>
    /// <param name="Username">Username</param>
    /// <param name="Password">Password</param>
    public record GetUserCredentialsQuery(string Username, string Password) : IBaseQuery
    {
        //TODO: Getting like this to simplify (when in time, create two tables to refer to roles as nxn or 1xn);
        public string Script => @"SELECT 
                                    u.id as UserId, 
                                    u.token_id as TokenId, 
                                    u.role as Role,
                                    u.salt as Salt
                                 FROM users u
                                 WHERE u.name = @Username AND u.password = @Password;";

        public object Param => new { Username, Password };
    }
}
