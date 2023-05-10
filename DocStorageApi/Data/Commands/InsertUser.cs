using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// represents insert_user
    /// </summary>
    /// <param name="Name">Name</param>
    /// <param name="Password">Password</param>
    /// <param name="Role">User Role</param>
    /// <param name="Status">Status</param>
    /// <returns>Inserted user id</returns>
    public class InsertUserCommand : BaseCommand
    {
        public InsertUserCommand(string name, string password, string role, bool status)
        {
            Name = name;
            Password = password;
            Role = role;
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

        public bool Status { get; private set; }

        public override string Script => "SELECT insert_user(@Name, @Password, @Role, @Status)";
        public override object Param => new { Name, Password, Role, Status };

    }
}
