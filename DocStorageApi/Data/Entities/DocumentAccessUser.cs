using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorageApi.Data.Entities
{
    [Table(_Entities.DocumentAccessUsers)]
    public class DocumentAccessUser
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("document_id")]
        public Guid DocumentId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("granted_by_user")]
        public Guid GrantedByUser { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual Document Document { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
