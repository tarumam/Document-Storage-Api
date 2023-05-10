using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record RemoveUserFromAccessGroupRequest(
        [Required] Guid userId,
        [Required] Guid groupId);
}
