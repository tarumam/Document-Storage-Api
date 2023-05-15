using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class AssignUserToDocumentCommand : BaseCommand
    {
        /// <summary>
        /// Executes SELECT assign_user_to_document(@UserId, @DocId, @GrantedByUser);
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="docId">Document Id</param>
        /// <param name="grantedByUser">Granted by User Id</param>
        public AssignUserToDocumentCommand(Guid userId, Guid docId, Guid grantedByUser)
        {
            UserId = userId;
            DocId = docId;
            GrantedByUser = grantedByUser;
        }

        public override string Script => @"SELECT assign_user_to_document(@UserId, @DocId, @GrantedByUser)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
        new NpgsqlParameter<Guid>("@UserId", UserId),
        new NpgsqlParameter<Guid>("@DocId", DocId),
        new NpgsqlParameter<Guid>("@GrantedByUser", GrantedByUser)
        };

        [Required]
        public Guid UserId { get; private set; }

        [Required]
        public Guid DocId { get; private set; }

        [Required]
        public Guid GrantedByUser { get; private set; }
    }
}
