using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public class UpdateUserRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(10)]
        public string Role { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
