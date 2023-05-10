using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class RemoveDocumentAccessUser : BaseCommand
    {
        public RemoveDocumentAccessUser(Guid userId, Guid documentId)
        {
            UserId = userId;
            DocumentId = documentId;
        }

        [Required]
        public Guid UserId { get; private set; }

        [Required]
        public Guid DocumentId { get; private set; }

        public override string Script => "SELECT remove_document_access_user(@UserId, @DocumentId)";
        public override object Param => new { UserId, DocumentId };
    }
}
