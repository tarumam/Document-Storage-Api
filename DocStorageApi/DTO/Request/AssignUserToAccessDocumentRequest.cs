using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record AssignUserToAccessDocumentRequest([Required] Guid UserId, [Required] Guid DocumentId);
}
