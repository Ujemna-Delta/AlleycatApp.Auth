using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Infrastructure.Exceptions
{
    public class InvalidModelException(string message, IEnumerable<ValidationResult> errors) : Exception(message)
    {
        public IEnumerable<ValidationResult> Errors => errors;
    }
}
