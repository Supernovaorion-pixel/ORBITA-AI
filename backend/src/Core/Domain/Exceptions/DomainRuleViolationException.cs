namespace OrbitaAI.Core.Domain.Exceptions;

/// <summary>
/// Signale la violation d'un invariant du Domaine qui ne relève pas d'un cas plus spécifique
/// déjà représenté par une autre exception de ce dossier (ex. <see cref="OrganizationMismatchException"/>).
/// Réservée à un invariant structurel qui ne devrait jamais pouvoir être rompu par un usage
/// normal du système ; un échec métier attendu reste représenté par le Result Pattern
/// (architecture/ERROR_HANDLING.md §2).
/// </summary>
public sealed class DomainRuleViolationException : DomainException
{
    /// <summary>Code stable identifiant la règle violée (ex. "Invoice.CustomerRequired").</summary>
    public string RuleCode { get; }

    public DomainRuleViolationException(string ruleCode, string message) : base(message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        RuleCode = ruleCode;
    }
}
