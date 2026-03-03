using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;
using RiskClassification.Domain.Rules;

namespace RiskClassification.Application.Services;

public sealed class RiskClassifier
{
    private readonly IReadOnlyList<IRiskRule> _rules;

    public RiskClassifier(IEnumerable<IRiskRule> rules)
    {
        _rules = rules.ToList();
    }

    public RiskCategory Classify(Trade trade)
    {
        foreach (var rule in _rules)
            if (rule.IsMatch(trade))
                return rule.Category;

        throw new InvalidOperationException("No matching risk rule found.");
    }
}