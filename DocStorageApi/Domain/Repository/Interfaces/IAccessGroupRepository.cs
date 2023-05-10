using DocStorageApi.Data.Commands;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IAccessGroupRepository
    {
        Task<IEnumerable<AccessGroupResponse>> GetAllAccessGroupsAsync();
        Task<CommandResult<int>> AddAccessGroupAsync(InsertAccessGroup command);
        Task<CommandResult<int>> UpdateAccessGroupAsync(UpdateAccessGroupCommand command);
        Task<CommandResult<int>> AssignUserToAccessGroupAsync(AssignUserToAccessGroupCommand command);
        Task<CommandResult<int>> RemoveUserFromAccessGroupAsync(RemoveUserAccessGroupCommand command);
    }
}
