using DocStorageApi.Data.Commands;
using System.ComponentModel.DataAnnotations;


public class RemoveUserAccessGroupCommand : BaseCommand
{
    /// <summary>
    /// Executes remove_user_access_group(@UserId, @GroupId)
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="groupId">UserGroupId</param>
    public RemoveUserAccessGroupCommand(Guid userId, Guid groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }

    [Required]
    public Guid UserId { get; }

    [Required]
    public Guid GroupId { get; }

    public override string Script => "SELECT remove_user_access_group(@UserId, @GroupId)";
    public override object Param => new { UserId, GroupId };
}