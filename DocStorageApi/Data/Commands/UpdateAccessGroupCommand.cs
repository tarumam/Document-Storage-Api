using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    /// <summary>
    /// Represents update_access_group
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="name">Name</param>
    /// <param name="status">status</param>
    public class UpdateAccessGroupCommand : BaseCommand
    {
        public UpdateAccessGroupCommand(Guid id, string name, bool status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        public Guid Id { get; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; }

        public bool Status { get; }
        public override string Script => @"SELECT update_access_group(@Id, @Name, @Status)";
        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("Id", Id),
            new NpgsqlParameter("Name", Name),
            new NpgsqlParameter("Status", Status),
        };

    }
};
