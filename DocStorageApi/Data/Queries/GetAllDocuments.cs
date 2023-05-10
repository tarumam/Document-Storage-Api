namespace DocStorageApi.Data.Queries
{
    public record GetAllDocuments : IBaseQuery
    {
        public string Script => @"SELECT 
                                    d.id as Id,
                                    d.file_path as FilePath,
                                    d.name as Name,
                                    d.category as Category,
                                    d.description as Description,
                                    d.posted_at as PostedAt,
                                    d.status as Status, 
                                    u.name as CreatedByUserName,
                                    d.created_at as CreatedAt
                                  FROM documents d 
                                  JOIN users u ON d.created_by_user = u.id;
                                    ";

        public object Param => new { };
    }
}

