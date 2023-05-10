namespace DocStorageApi.Data.Queries
{
    /// <summary>
    /// Query get all groups associated to a document
    /// </summary>
    public record GetAllGroupsAssociatedToDocument(Guid DocumentId) : IBaseQuery
    {
        public string Script => @"
                                SELECT
                                    d.id as DocumentId,
                                    d.name AS DocumentName,
                                    u.name AS GrantedByUserName,
                                    ag.name AS AccessGroupName
                                FROM
                                    documents d
                                    JOIN document_access_groups dag ON d.id = dag.document_id
                                    JOIN access_groups ag ON dag.access_group_id = ag.id
                                    JOIN users u ON dag.granted_by_user = u.id
                                WHERE
                                    d.id = @DocumentId;";

        public object Param => new { DocumentId };
    }
}
