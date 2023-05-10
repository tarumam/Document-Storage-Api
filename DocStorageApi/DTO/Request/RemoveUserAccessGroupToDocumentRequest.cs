using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record RemoveAccessGroupToDocumentRequest(
        [Required] Guid DocumentId,
        [Required] Guid AccessGroupId);
}
