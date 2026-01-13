namespace Domain.Abstractions;

/// <summary>
/// 操作結果を表すrecord（C# 14: record + コレクション式）
/// </summary>
public record Result(bool IsSuccess, IReadOnlyList<string> ErrorMessages)
{
    public static Result Success() => new(true, []);
    public static Result Failure(params string[] errors) => new(false, errors);
    public static Result Failure(IEnumerable<string> errors) => new(false, errors.ToList());
}

/// <summary>
/// 値を持つ操作結果を表すrecord
/// </summary>
public record Result<T>(bool IsSuccess, T? Value, IReadOnlyList<string> ErrorMessages)
    : Result(IsSuccess, ErrorMessages)
{
    public static Result<T> Success(T value) => new(true, value, []);
    public static new Result<T> Failure(params string[] errors) => new(false, default, errors);
    public static new Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors.ToList());
}
