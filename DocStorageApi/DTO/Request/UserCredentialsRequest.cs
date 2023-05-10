using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record UserCredentialsRequest(
        [Required][MaxLength(50)] string Username,
        [Required][MaxLength(100)] string HashedPassword);
}
