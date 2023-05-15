using Npgsql;

namespace DocStorageApi.Data.Commands
{

    public class RemoveDocumentAccessGroup : BaseCommand
    {
        /// <summary>
        ///  Executes SELECT remove_document_access_groups(@DocumentId, @AccessGroupId)
        /// </summary>
        /// <param name="documentId">DocumentId</param>
        /// <param name="accessGroupId">UserAccessGroupId</param>
        public RemoveDocumentAccessGroup(Guid documentId, Guid accessGroupId)
        {
            DocumentId = documentId;
            AccessGroupId = accessGroupId;
        }

        public Guid DocumentId { get; private set; }

        public Guid AccessGroupId { get; private set; }

        public override string Script => @"SELECT remove_document_access_groups(@DocumentId, @AccessGroupId)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@DocumentId", DocumentId),
            new NpgsqlParameter("@AccessGroupId", AccessGroupId)
        };
    }
}
