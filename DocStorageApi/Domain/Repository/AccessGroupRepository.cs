using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class AccessGroupRepository : BaseRepository<AccessGroupRepository>, IAccessGroupRepository
    {
        public AccessGroupRepository(DbSession session, ILogger<AccessGroupRepository> logger) : base(session, logger)
        {
        }

        public async Task<CommandResult<int>> AddAccessGroupAsync(InsertAccessGroup command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<CommandResult<int>> UpdateAccessGroupAsync(UpdateAccessGroupCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<CommandResult<int>> AssignUserToAccessGroupAsync(AssignUserToAccessGroupCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<CommandResult<int>> RemoveUserFromAccessGroupAsync(RemoveUserAccessGroupCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<IEnumerable<AccessGroupResponse>> GetAllAccessGroupsAsync()
        {
            var accessGroup = new GetAllAccessGroups();
            return await QueryAsync<AccessGroupResponse>(accessGroup.Script, accessGroup.Param);
        }
    }
}
