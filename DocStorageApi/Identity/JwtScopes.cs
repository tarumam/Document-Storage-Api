namespace DocStorageApi.Identity
{
    public static class JwtScopes
    {
        public const string JwtScopeRegular = "Regular";
        public const string JwtScopeManager = "Manager";
        public const string JwtScopeAdmin = "Admin";
    }

    //TODO: change it by polices
    public static class CanAccess
    {
        public const string AdminAccess = $"{JwtScopes.JwtScopeAdmin}";
        public const string ManagerAccess = $"{JwtScopes.JwtScopeManager},{JwtScopes.JwtScopeAdmin}";
        public const string RegularAccess = $"{JwtScopes.JwtScopeRegular},{JwtScopes.JwtScopeManager},{JwtScopes.JwtScopeAdmin}";
    }
}
