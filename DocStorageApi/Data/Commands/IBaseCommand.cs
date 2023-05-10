using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public interface IBaseCommand
    {
        public string Script { get; }
        public object Param { get; }
        public bool IsValid();
        public List<ValidationResult> ValidationResults { get; set; }
    }
}
