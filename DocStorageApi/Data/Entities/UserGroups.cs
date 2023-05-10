using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorageApi.Data.Entities
{

    [Table(_Entities.UserAccessGroups)]
    public class UserGroups
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("granted_by_user")]
        public Guid GrantedByUser { get; set; }

        [Column("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; } = null!;
    }
}
