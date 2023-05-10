namespace DocStorageApi.Data.Queries
{
    public record GetDocumentsRelatedToUsers(Guid UserId) : IBaseQuery
    {
        public string Script => @"
                                    SELECT 
                                      u.id as UserId,
                                      u.name as UserName,
                                      dau.created_at as GrantedAt,
                                      d.id as DocumentId,
                                      d.name as DocumentName,
                                      d.description as DocumentDescription
                                    FROM 
                                      document_access_users dau
                                      JOIN users u ON dau.user_id = u.id
                                      JOIN documents d ON dau.document_id = d.id
                                    WHERE 
                                      u.id = @UserId;
                                  ";
        public object Param => new { UserId };
    }
}
