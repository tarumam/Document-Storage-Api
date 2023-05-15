using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Commands
{
    public abstract class BaseCommand : IBaseCommand
    {
        public abstract string Script { get; }
        public abstract List<NpgsqlParameter> Parameters { get; }
        public List<ValidationResult> ValidationResults { get; set; }
        public bool IsValid()
        {
            var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                ValidationResults = validationResults;
            };
            return !validationResults.Any();
        }
    }
}
