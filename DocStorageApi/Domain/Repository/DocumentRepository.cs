using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class DocumentRepository : BaseRepository<DocumentRepository>, IDocumentRepository
    {
        public DocumentRepository(DbSession session, ILogger<DocumentRepository> logger) : base(session, logger) { }

        public async Task<CommandResult<Guid?>> AddNewDocumentAsync(InsertDocumentCommand command)
        {
            return await ExecuteScalarCommand<Guid?>(command);
        }

        public async Task<CommandResult<int>> DisableDocumentAsync(DisableDocumentCommand command)
        {
            var result = await ExecuteScalarCommand<int>(command);
            return result;
        }

        public async Task<IEnumerable<DocumentsResponse>> GetAllDocumentsAsync()
        {
            var query = new GetAllDocuments();
            return await QueryAsync<DocumentsResponse>(query.Script);
        }

        public async Task<IEnumerable<DocumentPermissionsWithUserResponse>> ListDocumentPermissionsWithUsers(Guid? userId)
        {
            var query = new GetDocumentPermissionsByUserQuery(userId);
            return await QueryAsync<DocumentPermissionsWithUserResponse>(query.Script, query.Param);
        }
    }
}
