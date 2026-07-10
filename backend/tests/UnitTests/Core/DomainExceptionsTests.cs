using OrbitaAI.Core.Domain.Exceptions;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class DomainExceptionsTests
{
    [Fact]
    public void DomainRuleViolationException_ExposesRuleCodeAndMessage()
    {
        var exception = new DomainRuleViolationException("Invoice.CustomerRequired", "Une facture doit référencer un client.");

        Assert.Equal("Invoice.CustomerRequired", exception.RuleCode);
        Assert.Equal("Une facture doit référencer un client.", exception.Message);
        Assert.IsAssignableFrom<DomainException>(exception);
    }

    [Fact]
    public void DomainRuleViolationException_WithNullRuleCode_ThrowsArgumentNullException()
    {
        // ArgumentException.ThrowIfNullOrWhiteSpace lève spécifiquement ArgumentNullException
        // (sous-type d'ArgumentException) pour une valeur null — Assert.Throws<T> exigeant un
        // type exact en xUnit, ce cas est distingué des cas vide/espaces ci-dessous.
        Assert.Throws<ArgumentNullException>(() => new DomainRuleViolationException(null!, "message"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DomainRuleViolationException_WithEmptyOrWhiteSpaceRuleCode_ThrowsArgumentException(string ruleCode)
    {
        Assert.Throws<ArgumentException>(() => new DomainRuleViolationException(ruleCode, "message"));
    }

    [Fact]
    public void OrganizationMismatchException_ExposesBothOrganizationIds()
    {
        var expected = Guid.NewGuid();
        var actual = Guid.NewGuid();

        var exception = new OrganizationMismatchException(expected, actual);

        Assert.Equal(expected, exception.ExpectedOrganizationId);
        Assert.Equal(actual, exception.ActualOrganizationId);
        Assert.IsAssignableFrom<DomainException>(exception);
    }
}
