using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class DocAccessGroupRepository : BaseRepository<DocAccessGroupRepository>, IDocAccessGroupRepository
    {
        public DocAccessGroupRepository(DbSession session, ILogger<DocAccessGroupRepository> logger) : base(session, logger)
        {
        }

        public async Task<CommandResult<int>> GrantDocPermissionForGroupAsync(AssignGroupToDocumentCommand command)
        {
            return await ExecuteScalarCommand<int>(command);
        }

        public async Task<IEnumerable<DocumentAccessGroupsResponse>> GetAllGroupsAssociatedToDocumentAsync(Guid documentId)
        {
            var groupsRelatedToDoc = new GetAllGroupsAssociatedToDocument(documentId);
            return await QueryAsync<DocumentAccessGroupsResponse>(groupsRelatedToDoc.Script, groupsRelatedToDoc.Param);
        }

        public async Task<IEnumerable<DocumentAccessGroupsResponse>> GetAllDocumentsAssociatedToGroupAsync(Guid groupId)
        {
            var docsAssociatedToGroup = new GetAllDocumentsAssociatedToGroup(groupId);
            return await QueryAsync<DocumentAccessGroupsResponse>(docsAssociatedToGroup.Script, docsAssociatedToGroup.Param);
        }

        public async Task<CommandResult<int>> RemoveDocPermissionFromGroupAsync(RemoveDocumentAccessGroup command)
        {
            return await ExecuteScalarCommand<int>(command);
        }
    }
}
