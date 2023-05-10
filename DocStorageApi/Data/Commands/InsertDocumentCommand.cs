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
        public override object Param => new { FilePath, Name, Category, Description, PostedAt, Status, CreatedByUser };
    }
}
