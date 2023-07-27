using FluentValidation.Results;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public void HandleResultWithValidationException(string defaultMessage)
    {
        if (Errors.Length > 0)
        {
            var validationFailures = new List<ValidationFailure>();
            int index = 0;
            foreach (var error in Errors)
            {
                validationFailures.Add(new ValidationFailure($"general{index}", error));
                index++;
            }
            throw new ValidationException(validationFailures);
        }
        else
        {
            throw new Exception(defaultMessage);
        }
    }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}
