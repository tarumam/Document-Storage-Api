using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class AssignGroupToDocumentCommand : BaseCommand
    {
        /// <summary>
        /// Executes SELECT assign_group_to_document(@AccessGroupId, @DocId, @GrantedByUser)
        /// </summary>
        /// <param name="accessGroupId">AccessGroupId</param>
        /// <param name="docId">DocId</param>
        /// <param name="grantedByUser">GrantedByUser</param>
        /// <returns>Number of affected rows</returns>
        public AssignGroupToDocumentCommand(Guid accessGroupId, Guid docId, Guid grantedByUser)
        {
            AccessGroupId = accessGroupId;
            DocId = docId;
            GrantedByUser = grantedByUser;
        }
        public override string Script => "SELECT assign_group_to_document(@AccessGroupId, @DocId, @GrantedByUser)";

        public override object Param => new { AccessGroupId, DocId, GrantedByUser };

        public Guid AccessGroupId { get; }
        public Guid DocId { get; }
        public Guid GrantedByUser { get; }
    }
}
