using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public class CommandResult<T>
    {
        public CommandResult()
        {

        }
        public CommandResult(T data)
        {
            Data = data;
            Succeeded = true;
        }
        public CommandResult(string errorMessage, string commandName)
        {
            var error = new ValidationResult(errorMessage, new List<string>() { commandName });
            Errors = new List<ValidationResult> { error };
            Succeeded = false;
        }

        public CommandResult(IEnumerable<ValidationResult> validationMessages)
        {
            Errors = validationMessages;
            Succeeded = false;
        }

        public bool Succeeded { get; private set; } = false;
        public T Data { get; private set; }
        public IEnumerable<ValidationResult> Errors { get; private set; } = new List<ValidationResult>();

        public void SetData(T data)
        {
            Data = data;
        }

        public void SetError(T data)
        {
            Data = data;
        }
    }

}
