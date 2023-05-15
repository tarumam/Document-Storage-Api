using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class DisableUserCommand : BaseCommand
    {
        /// <summary>
        /// Represents disable_user
        /// </summary>
        /// <param name="Id">UserId</param>
        /// <returns>Number of affected rows</returns>
        public DisableUserCommand(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; }

        public override string Script => "SELECT disable_user(@Id)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter<Guid>("Id", Id)
        };
    }
}
