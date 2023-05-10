using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// Represents update_token_id
    /// </summary>
    /// <param name="Id">UserId</param>
    /// <param name="TokenId">TokenId</param>
    /// <returns>Number of affected rows</returns>
    public class UpdateUserTokenIdCommand : BaseCommand
    {
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
        public override object Param => new { Id, TokenId };
    }
}
