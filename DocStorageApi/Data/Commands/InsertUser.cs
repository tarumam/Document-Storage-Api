using Npgsql;
using NpgsqlTypes;
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
        [EmailAddress]
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

        public override string Script => "SELECT insert_user(@Name, @Password, @Role, @Salt, @Status)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter<string>("@Name", NpgsqlDbType.Varchar) { Value = Name, Size=50},
            new NpgsqlParameter<string>("@Password", NpgsqlDbType.Varchar) { Value = Password, Size=100},
            new NpgsqlParameter<string>("@Role", NpgsqlDbType.Varchar) { Value = Role, Size=50},
            new NpgsqlParameter<string>("@Salt", NpgsqlDbType.Varchar) { Value = Salt , Size = 10},
            new NpgsqlParameter<bool>("@Status", NpgsqlDbType.Boolean) { Value = Status }
        };

    }
}
