using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models.Validation
{
    public record ModelError(string Message, IEnumerable<ValidationResult>? Errors);
}
