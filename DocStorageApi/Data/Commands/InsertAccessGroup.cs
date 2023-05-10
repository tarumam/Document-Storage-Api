using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{

    public class InsertAccessGroup : BaseCommand
    {
        /// <summary>
        /// Executes SELECT insert_access_group(@Name, @Status)
        /// </summary>
        /// <param name="name">Name of Access Group</param>
        /// <param name="status">Status</param>
        public InsertAccessGroup(string name, bool status = true)
        {
            Name = name;
            Status = status;
        }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; private set; }

        public bool Status { get; private set; }

        public override string Script => @"SELECT insert_access_group(@Name, @Status)";
        public override object Param => new { Name, Status };

    }
}
