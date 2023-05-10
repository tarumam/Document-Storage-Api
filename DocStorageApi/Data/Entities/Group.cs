using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorageApi.Data.Entities
{
    [Table(_Entities.AccessGroups)]
    public class Group
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}