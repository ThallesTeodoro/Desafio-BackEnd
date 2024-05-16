using FluentValidation.Results;

namespace DesafioBackEnd.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures has ocurred.")
    {
        Errors = failures
            .Distinct()
            .Select(failure => new Dictionary<string, string>() { { failure.PropertyName, failure.ErrorMessage } })
            .ToArray();
    }

    public IReadOnlyDictionary<string, string>[] Errors { get; }
}
