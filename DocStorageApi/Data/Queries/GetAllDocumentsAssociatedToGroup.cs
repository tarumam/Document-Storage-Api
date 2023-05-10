namespace DocStorageApi.Data.Queries
{
    public record GetAllDocumentsAssociatedToGroup(Guid AccessGroupId) : IBaseQuery
    {
        public string Script => @"
                                    SELECT
                                      d.id AS DocumentId,
                                      d.name AS DocumentName,
                                      d.category As Category,
                                      d.description As Description,
                                      d.posted_at As PostedAt,
                                      d.status AS Status,
                                      u.name AS CreatedByUserName,
                                      ag.name AS AccessGroupName
                                    FROM
                                      documents d
                                      JOIN document_access_groups dag ON d.id = dag.document_id
                                      JOIN access_groups ag ON dag.access_group_id = ag.id
                                      JOIN user_access_groups uag ON ag.id = uag.access_group_id
                                      JOIN users u ON d.created_by_user = u.id
                                    WHERE
                                      ag.id = @AccessGroupId
                                    ORDER BY
                                      d.posted_at DESC;";

        public object Param => new { AccessGroupId };
    }
}
