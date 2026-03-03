using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Domain.Rules;

public sealed class LowRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.LOWRISK;
    public bool IsMatch(Trade trade) => trade.Value < 1_000_000m;
}