namespace DocStorageApi.Data.Queries
{
    /// <summary>
    /// Query that gets all data on access_groups table
    /// </summary>
    public record GetAllAccessGroups : IBaseQuery
    {
        public string Script => @"select id, name, status from access_groups g";
        public object Param => new { };
    }
}
