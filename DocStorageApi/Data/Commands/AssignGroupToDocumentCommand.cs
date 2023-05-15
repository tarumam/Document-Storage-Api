using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class AssignGroupToDocumentCommand : BaseCommand
    {
        /// <summary>
        /// Assigns a document to an access group.
        /// </summary>
        /// <param name="accessGroupId">The ID of the access group to assign the document to.</param>
        /// <param name="docId">The ID of the document to assign.</param>
        /// <param name="grantedByUser">The ID of the user who granted access to the group.</param>
        public AssignGroupToDocumentCommand(Guid accessGroupId, Guid docId, Guid grantedByUser)
        {
            AccessGroupId = accessGroupId;
            DocId = docId;
            GrantedByUser = grantedByUser;
        }

        public override string Script => "SELECT assign_group_to_document(@AccessGroupId, @DocId, @GrantedByUser)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter<Guid>("AccessGroupId", AccessGroupId),
            new NpgsqlParameter<Guid>("DocId", DocId),
            new NpgsqlParameter<Guid>("GrantedByUser", GrantedByUser)
        };

        [Required]
        public Guid AccessGroupId { get; }

        [Required]
        public Guid DocId { get; }

        [Required]
        public Guid GrantedByUser { get; }
    }
}
