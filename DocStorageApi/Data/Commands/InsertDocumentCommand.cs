using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{

    public class InsertDocumentCommand : BaseCommand
    {
        /// <summary>
        /// Executes insert_document(@FilePath, @Name, @Category, @Description, @PostedAt, @Status, @CreatedbyUser)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="description"></param>
        /// <param name="postedAt"></param>
        /// <param name="status"></param>
        /// <param name="createdByUser"></param>
        public InsertDocumentCommand(string filePath, string name, string category, string description, DateTime postedAt, bool status, Guid createdByUser)
        {
            FilePath = filePath;
            Name = name;
            Category = category;
            Description = description;
            PostedAt = postedAt;
            Status = status;
            CreatedByUser = createdByUser;
        }

        [Required]
        public string FilePath { get; private set; }

        public string Name { get; private set; }

        public string Category { get; private set; }

        public string Description { get; private set; }

        [Required]
        public DateTime PostedAt { get; private set; }

        public bool Status { get; private set; }

        [Required]
        public Guid CreatedByUser { get; private set; }

        public override string Script => "SELECT insert_document(@FilePath, @Name, @Category, @Description, @PostedAt, @Status, @CreatedbyUser)";

        public override List<NpgsqlParameter> Parameters => new List<NpgsqlParameter>()
        {
            new NpgsqlParameter("@FilePath", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = FilePath },
            new NpgsqlParameter("@Name", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = string.IsNullOrEmpty(Name) ? DBNull.Value : Name },
            new NpgsqlParameter("@Category", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = string.IsNullOrEmpty(Category) ? DBNull.Value : Category },
            new NpgsqlParameter("@Description", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = string.IsNullOrEmpty(Description) ? DBNull.Value : Description },
            new NpgsqlParameter("@PostedAt", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = PostedAt },
            new NpgsqlParameter("@Status", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = Status },
            new NpgsqlParameter("@CreatedbyUser", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = CreatedByUser }
        };
    }
}
