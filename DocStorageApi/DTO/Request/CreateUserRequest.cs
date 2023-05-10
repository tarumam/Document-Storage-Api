using DocStorageApi.Identity;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public class CreateUserRequest
    {

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(10)]
        public string Role { get; set; }
    }
}
