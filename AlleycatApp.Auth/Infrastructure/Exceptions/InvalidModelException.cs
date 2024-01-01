using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Validation;

namespace AlleycatApp.Auth.Infrastructure.Exceptions
{
    public class InvalidModelException(string message, IEnumerable<ValidationResult> errors) : Exception(message)
    {
        public IEnumerable<ValidationResult> Errors { get; } = errors;

        public ModelError ModelError => new(base.Message, Errors);
    }
}
