using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Domain.Rules;

public sealed class MediumRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.MEDIUMRISK;

    public bool IsMatch(Trade trade) =>
        trade.Value >= 1_000_000m && trade.ClientSector == ClientSector.Public;
}