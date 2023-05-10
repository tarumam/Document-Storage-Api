using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Entities
{
    [Table(_Entities.Users)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("token_id")]
        public string? TokenId { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}