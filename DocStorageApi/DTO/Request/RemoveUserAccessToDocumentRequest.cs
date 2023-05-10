using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record RemoveUserAccessToDocumentRequest(
        [Required] Guid DocumentId,
        [Required] Guid UserId);
}
