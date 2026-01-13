namespace Domain.Exceptions;

/// <summary>
/// バリデーション例外
/// </summary>
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = [message];
    }

    public ValidationException(IEnumerable<string> errors) : base(string.Join(", ", errors))
    {
        Errors = errors.ToList();
    }
}
