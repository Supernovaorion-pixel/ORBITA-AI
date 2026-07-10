using OrbitaAI.Core.Common.Errors;

namespace OrbitaAI.Core.Common.Results;

/// <summary>
/// Résultat d'une opération, sans valeur associée. Porte de façon explicite le succès ou
/// l'échec d'une opération, en remplacement des exceptions pour les échecs métier attendus
/// (architecture/ERROR_HANDLING.md §2 ; cf. aussi <see cref="Result{TValue}"/> pour un résultat
/// porteur d'une valeur).
/// </summary>
public class Result
{
    /// <summary>Indique si l'opération a réussi.</summary>
    public bool IsSuccess { get; }

    /// <summary>Indique si l'opération a échoué (strict inverse de <see cref="IsSuccess"/>).</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Erreur associée à l'opération. Vaut toujours <see cref="Error.None"/> lorsque
    /// <see cref="IsSuccess"/> est vrai.
    /// </summary>
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Un résultat en succès ne peut pas porter d'erreur.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Un résultat en échec doit porter une erreur explicite.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>Construit un résultat en succès, sans valeur.</summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>Construit un résultat en échec, portant l'erreur fournie.</summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>Construit un résultat en succès portant la valeur fournie (cf. <see cref="Result{TValue}"/>).</summary>
    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    /// <summary>Construit un résultat en échec typé, portant l'erreur fournie (cf. <see cref="Result{TValue}"/>).</summary>
    public static Result<TValue> Failure<TValue>(Error error) => Result<TValue>.Failure(error);
}

/// <summary>
/// Résultat d'une opération porteur d'une valeur en cas de succès (architecture/ERROR_HANDLING.md §2).
/// La valeur n'est accessible que si l'opération a réussi ; y accéder après un échec est une
/// erreur de programmation, jamais un cas attendu, et lève donc une exception.
/// </summary>
/// <typeparam name="TValue">Type de la valeur portée en cas de succès.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Valeur portée par un résultat réussi. Lève <see cref="InvalidOperationException"/>
    /// si le résultat est en échec : la présence d'une valeur n'est jamais garantie côté appelant
    /// sans vérification préalable de <see cref="Result.IsSuccess"/>.
    /// </summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Impossible d'accéder à la valeur d'un résultat en échec.");

    /// <summary>Construit un résultat réussi portant <paramref name="value"/>.</summary>
    public static Result<TValue> Success(TValue value) => new(value, true, Error.None);

    /// <summary>Construit un résultat en échec, portant l'erreur fournie.</summary>
    public static new Result<TValue> Failure(Error error) => new(default, false, error);

    /// <summary>Conversion implicite pratique : une valeur devient un résultat réussi.</summary>
    public static implicit operator Result<TValue>(TValue value) => Success(value);
}
