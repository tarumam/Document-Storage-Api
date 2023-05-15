using Npgsql;
using NpgsqlTypes;

namespace DocStorageApi.Data.Commands
{
    public class AssignUserToAccessGroupCommand : BaseCommand
    {
        /// <summary>
        /// Executes SELECT assign_user_to_access_group(@UserId, @AccessGroupId, @GrantedBy)
        /// </summary>
        /// <param name="userId">The ID of the user to assign.</param>
        /// <param name="accessGroupId">The ID of the access group to assign the user to.</param>
        /// <param name="grantedBy">The ID of the user who granted access to the group.</param>
        public AssignUserToAccessGroupCommand(Guid userId, Guid accessGroupId, Guid grantedBy)
        {
            UserId = userId;
            AccessGroupId = accessGroupId;
            GrantedBy = grantedBy;
        }

        public Guid UserId { get; }
        public Guid AccessGroupId { get; }
        public Guid GrantedBy { get; }

        public override string Script => @"SELECT assign_user_to_access_group(@UserId, @AccessGroupId, @GrantedBy)";

        public override List<NpgsqlParameter> Parameters => new()
        {
            new NpgsqlParameter<Guid>("UserId", NpgsqlDbType.Uuid) { TypedValue = UserId },
            new NpgsqlParameter<Guid>("AccessGroupId", NpgsqlDbType.Uuid) { TypedValue = AccessGroupId },
            new NpgsqlParameter<Guid>("GrantedBy", NpgsqlDbType.Uuid) { TypedValue = GrantedBy },
        };
    }
}
