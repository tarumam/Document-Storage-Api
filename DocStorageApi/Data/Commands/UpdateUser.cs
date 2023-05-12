using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class UpdateUserCommand : BaseCommand
    {
        /// <summary>
        /// Represents update_user
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="name">Name</param>
        /// <param name="password">Password</param>
        /// <param name="role">User Role</param>
        /// <param name="salt">Pass Salt</param>
        /// <param name="status">Status</param>
        /// <returns>Number of affected rows</returns>
        public UpdateUserCommand(Guid id, string name, string password, string role, string salt, bool status)
        {
            Id = id;
            Name = name;
            Password = password;
            Role = role;
            Salt = salt;
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

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Salt { get; private set; }

        public bool Status { get; private set; }
        public override string Script => "SELECT update_user(@Id, @Name, @Password, @Role, @Status)";
        public override object Param => new { Id, Name, Password, Role, Status };

    }
}
