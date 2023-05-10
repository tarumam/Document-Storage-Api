using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class DisableDocumentCommand : BaseCommand
    {
        public DisableDocumentCommand(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; }
        public override string Script => "SELECT disable_document(@Id)";
        public override object Param => new { Id };

    }
}
