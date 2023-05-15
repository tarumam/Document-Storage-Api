using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class DisableDocumentCommand : BaseCommand
    {
        /// <summary>
        /// Executes SELECT disable_document(@Id)
        /// </summary>
        /// <param name="Id">documentId</param>
        /// <returns>Number of affected rows</returns>
        public DisableDocumentCommand(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; }

        public override string Script => "SELECT disable_document(@Id)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter<Guid>("@Id", Id)
        };
    }
}
