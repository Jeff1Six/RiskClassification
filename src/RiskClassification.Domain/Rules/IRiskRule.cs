using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Domain.Rules;

public interface IRiskRule
{
    RiskCategory Category { get; }
    bool IsMatch(Trade trade);
}