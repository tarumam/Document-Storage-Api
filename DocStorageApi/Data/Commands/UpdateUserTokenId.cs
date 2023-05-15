using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class UpdateUserTokenIdCommand : BaseCommand
    {
        /// <summary>
        /// Represents update_token_id
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="TokenId">TokenId</param>
        /// <returns>Number of affected rows</returns>
        public UpdateUserTokenIdCommand(Guid id, string tokenId)
        {
            Id = id;
            TokenId = tokenId;
        }

        [Required]
        public Guid Id { get; private set; }

        [MaxLength(100)]
        public string TokenId { get; private set; }
        public override string Script => "SELECT update_token_id(@Id, @TokenId)";
        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter<Guid>("Id", Id),
            new NpgsqlParameter<string>("TokenId", TokenId)
        };
    }
}
