using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Queries
{
    public record GetDocumentPermissionsByUserQuery(Guid? UserId) : IBaseQuery
    {
        public string Script => @"
                                    SELECT 
                                        u2.id as UserId,
                                        u2.name as Username,    
                                        CASE WHEN dau.id IS NULL THEN 'G' ELSE 'U' END as AccessType,
                                        NULL as AccessGroupName,
                                        NULL as AccessGroupId,
                                        d.id as DocumentId,
                                        d.file_path as FilePath,
                                        d.name as Name,
                                        d.category as Category,
                                        d.description as Description,
                                        d.posted_at as PostedAt,
                                        d.status as Status,
                                        u.name as CreatedByUserName,
                                        d.created_at as CreatedAt
                                    FROM 
                                        documents d 
                                        JOIN users u ON d.created_by_user = u.id
                                        JOIN document_access_users dau on d.id = dau.document_id 
                                        JOIN users u2 on dau.user_id = u2.id 
		                                where @UserId is null OR U2.id = @UserId

                                UNION all

                                    SELECT 
    	                                u3.id as UserId,
    	                                u3.name as Username,  
    	                                CASE WHEN dag.id IS NULL THEN 'U' ELSE 'G' END as AccessType,
                                        ag.name as AccessGroupName,
                                        ag.id as AccessGroupId,
                                        d.id as DocumentId,
                                        d.file_path as FilePath,
                                        d.name as Name,
                                        d.category as Category,
                                        d.description as Description,
                                        d.posted_at as PostedAt,
                                        d.status as Status,
                                        u.name as CreatedByUserName,
                                        d.created_at as CreatedAt
                                    FROM 
                                        documents d 
                                        JOIN users u ON d.created_by_user = u.id
                                        INNER JOIN document_access_groups dag on d.id = dag.document_id 
                                        INNER JOIN access_groups ag on dag.access_group_id = ag.id 
                                        INNER JOIN user_access_groups uag on ag.id = uag.access_group_id
                                        INNER JOIN users u3 on uag.user_id  = u3.id
                                        where @UserId is null OR U3.id = @UserId
                                    ";

        public object Param => new { UserId };
    }
}
