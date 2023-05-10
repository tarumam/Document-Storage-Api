using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// Represents update_user
    /// </summary>
    /// <param name="id">UserId</param>
    /// <param name="name">Name</param>
    /// <param name="password">Password</param>
    /// <param name="role">User Role</param>
    /// <param name="status">Status</param>
    /// <returns>Number of affected rows</returns>
    public class UpdateUserCommand : BaseCommand
    {
        public UpdateUserCommand(Guid id, string name, string password, string role, bool status)
        {
            Id = id;
            Name = name;
            Password = password;
            Role = role;
            Status = status;
        }


        [Required]
        public Guid Id { get; private set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; private set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; private set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Role { get; private set; }

        public bool Status { get; private set; }
        public override string Script => "SELECT update_user(@Id, @Name, @Password, @Role, @Status)";
        public override object Param => new { Id, Name, Password, Role, Status };

    }
}
