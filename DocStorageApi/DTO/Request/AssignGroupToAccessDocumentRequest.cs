using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record AssignGroupToAccessDocumentRequest([Required] Guid AccessGroupId, [Required] Guid DocumentId);
}
