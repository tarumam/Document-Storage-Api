using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record AssignUserToAccessGroupRequest(
        [Required] Guid userId,
        [Required] Guid accessGroupId);
}
