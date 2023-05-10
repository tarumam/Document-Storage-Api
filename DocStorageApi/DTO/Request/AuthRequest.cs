using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record AuthRequest(
        [Required][MaxLength(50)] string userName,
        [Required][MaxLength(50)] string password)
    {

    }
}
