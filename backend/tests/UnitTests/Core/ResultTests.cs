using OrbitaAI.Core.Common.Errors;
using OrbitaAI.Core.Common.Results;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class ResultTests
{
    [Fact]
    public void Success_HasNoError_AndIsNotFailure()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_CarriesTheProvidedError_AndIsNotSuccess()
    {
        var error = Error.Failure("Test.Failed", "Something went wrong.");

        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void GenericSuccess_ExposesValue()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void GenericFailure_AccessingValue_Throws()
    {
        var result = Result<int>.Failure(Error.Failure("Test.Failed", "Something went wrong."));

        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ProducesSuccessResult()
    {
        Result<string> result = "orbita-ai";

        Assert.True(result.IsSuccess);
        Assert.Equal("orbita-ai", result.Value);
    }

    [Fact]
    public void Validation_AggregatesValidationErrors_IntoASingleError()
    {
        var validationErrors = new[]
        {
            new ValidationError("Name", "Le nom est obligatoire."),
            new ValidationError("Email", "L'adresse n'est pas valide."),
        };

        var error = Error.Validation(validationErrors);

        Assert.Equal(ErrorType.Validation, error.Type);
        Assert.Equal(validationErrors, error.ValidationErrors);
        Assert.Contains("Name", error.Message);
        Assert.Contains("Email", error.Message);
    }

    [Fact]
    public void Validation_WithoutAnyError_Throws()
    {
        Assert.Throws<ArgumentException>(() => Error.Validation(Array.Empty<ValidationError>()));
    }
}
