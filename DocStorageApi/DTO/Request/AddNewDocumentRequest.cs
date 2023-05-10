using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record AddNewDocumentRequest(
        [Required][MaxLength(1000)] string FileKey,
        [MaxLength(50)] string Name,
        [MaxLength(500)] string Category,
        [MaxLength(500)] string Description,
        DateTime PostedAt);
}
