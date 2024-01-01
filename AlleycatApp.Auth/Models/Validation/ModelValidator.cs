using AlleycatApp.Auth.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models.Validation
{
    public static class ModelValidator
    {
        public static IEnumerable<ValidationResult> GetErrors<T>(T model) where T : class
        {
            var validationContext = new ValidationContext(model);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, errors, true);

            return errors;
        }

        public static void Validate<T>(T model) where T : class
        {
            var errors = GetErrors(model).ToArray();
            if (errors.Length != 0)
                throw new InvalidModelException($"Given model is invalid.", errors);
        }
    }
}
