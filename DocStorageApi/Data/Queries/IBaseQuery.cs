namespace DocStorageApi.Data.Queries
{
    public interface IBaseQuery
    {
        string Script { get; }
        object Param { get; }
    }
}
