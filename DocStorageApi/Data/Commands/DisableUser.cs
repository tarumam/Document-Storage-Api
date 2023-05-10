using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// Represents disable_user
    /// </summary>
    /// <param name="Id">UserId</param>
    /// <returns>Number of affected rows</returns>
    public class DisableUserCommand : BaseCommand
    {
        public DisableUserCommand(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; }

        public override string Script => "SELECT disable_user(@Id)";
        public override object Param => new { Id };
    }
}
