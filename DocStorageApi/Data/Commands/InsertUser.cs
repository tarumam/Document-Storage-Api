using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{

    public class InsertUserCommand : BaseCommand
    {
        /// <summary>
        /// represents insert_user
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="password">Password</param>
        /// <param name="role">User Role</param>
        /// <param name="salt">Pass salt</param>
        /// <param name="status">Status</param>
        /// <returns>Inserted user id</returns>
        public InsertUserCommand(string name, string password, string role, string salt, bool status)
        {
            Name = name;
            Password = password;
            Role = role;
            Salt = salt;
            Status = status;
        }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; private set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Password { get; private set; }

        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Role { get; private set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Salt { get; private set; }
        public bool Status { get; private set; }

        public override string Script => "SELECT insert_user(@Name, @Password, @Role, @Status)";
        public override object Param => new { Name, Password, Role, Status };

    }
}
