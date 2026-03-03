using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Domain.Rules;

public sealed class HighRiskRule : IRiskRule
{
    public RiskCategory Category => RiskCategory.HIGHRISK;

    public bool IsMatch(Trade trade) =>
        trade.Value >= 1_000_000m && trade.ClientSector == ClientSector.Private;
}