using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorageApi.Data.Entities
{
    [Table(_Entities.Documents)]
    public class Document
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("file_path")]
        public string? FilePath { get; set; }

        [Column("posted_at")]
        public DateTimeOffset? PostedDate { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
