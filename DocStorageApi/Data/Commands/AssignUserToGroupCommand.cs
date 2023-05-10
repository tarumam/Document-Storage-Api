using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// Assigns a user to an access group.
    /// </summary>
    /// <param name="UserId">The ID of the user to assign.</param>
    /// <param name="GroupId">The ID of the access group to assign the user to.</param>
    /// <param name="GrantedBy">The ID of the user who granted access to the group.</param>
    /// <returns>1 if successful; -1 if a unique violation occurs; -2 if a foreign key violation occurs.</returns>
    public class AssignUserToAccessGroupCommand : BaseCommand
    {
        public AssignUserToAccessGroupCommand(Guid userId, Guid accessGroupId, Guid grantedBy)
        {
            UserId = userId;
            AccessGroupId = accessGroupId;
            GrantedBy = grantedBy;
        }

        [Required]
        public Guid UserId { get; }

        [Required]
        public Guid AccessGroupId { get; }

        [Required]
        public Guid GrantedBy { get; }

        public override string Script => @"SELECT assign_user_to_access_group(@UserId, @AccessGroupId, @GrantedBy)";
        public override object Param => new { UserId, AccessGroupId, GrantedBy };

    }
}
