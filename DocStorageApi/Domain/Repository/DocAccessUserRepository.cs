using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class DocAccessUserRepository : BaseRepository<DocAccessUserRepository>, IDocAccessUserRepository
    {
        public DocAccessUserRepository(DbSession session, ILogger<DocAccessUserRepository> logger) : base(session, logger)
        {
        }

        public async Task<IEnumerable<DocumentsRelatedToUserResponse>> GetAllDocumentsAssociatedToUsersAsync(Guid userId)
        {
            var cmd = new GetDocumentsRelatedToUsers(userId);
            return await QueryAsync<DocumentsRelatedToUserResponse>(cmd.Script, cmd.Param);
        }

        public async Task<CommandResult<int>> RemoveDocPermissionForUserAsync(RemoveDocumentAccessUser command)
        {
            return await ExecuteScalarCommand<int>(command);
        }

        public async Task<CommandResult<int>> GrantDocPermissionForUserAsync(AssignUserToDocumentCommand command)
        {
            return await ExecuteScalarCommand<int>(command);
        }
    }
}
