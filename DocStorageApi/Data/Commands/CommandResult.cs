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
            Executed = data != null ? true : false;
        }
        public CommandResult(string errorMessage, string commandName)
        {
            var error = new ValidationResult(errorMessage, new List<string>() { commandName });
            Errors = new List<ValidationResult> { error };
            Executed = false;
        }

        public CommandResult(IEnumerable<ValidationResult> validationMessages)
        {
            Errors = validationMessages;
            Executed = false;
        }

        public bool Executed { get; private set; } = false;
        public T Data { get; private set; }
        public IEnumerable<ValidationResult> Errors { get; private set; } = new List<ValidationResult>();

        public void SetData(T data)
        {
            Data = data;
        }

        public void SetError(string error)
        {
            var validationError = new ValidationResult(error);
            Errors.Append(validationError); 
            Executed = false;
        }
    }

}
