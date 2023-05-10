using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorageApi.Data.Entities
{
    [Table(_Entities.DocumentAccessGroups)]
    public class DocumentAccessGroup
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("granted_by_user")]
        public Guid GrantedByUser { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}