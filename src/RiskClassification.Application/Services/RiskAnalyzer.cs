using RiskClassification.Application.DTOs;
using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Application.Services;

public sealed class RiskAnalyzer
{
    private sealed class Agg
    {
        public int Count;
        public decimal TotalValue;

        public Dictionary<string, decimal> ExposureByClient = new(StringComparer.OrdinalIgnoreCase);

        public string TopClient = "";
        public decimal TopExposure = 0m;
    }

    private readonly RiskClassifier _classifier;

    public RiskAnalyzer(RiskClassifier classifier) => _classifier = classifier;

    public (List<string> Categories, Dictionary<string, RiskSummaryDto> Summary) Analyze(IReadOnlyList<Trade> trades)
    {
        var categories = new List<string>(trades.Count);
        var agg = new Dictionary<RiskCategory, Agg>();

        foreach (var trade in trades)
        {
            var cat = _classifier.Classify(trade);
            categories.Add(cat.ToString());

            if (!agg.TryGetValue(cat, out var a))
            {
                a = new Agg();
                agg[cat] = a;
            }

            a.Count++;
            a.TotalValue += trade.Value;

            a.ExposureByClient.TryGetValue(trade.ClientId, out var current);
            var newExposure = current + trade.Value;
            a.ExposureByClient[trade.ClientId] = newExposure;

            if (newExposure > a.TopExposure)
            {
                a.TopExposure = newExposure;
                a.TopClient = trade.ClientId;
            }
        }

        var summary = new Dictionary<string, RiskSummaryDto>(StringComparer.OrdinalIgnoreCase);

        foreach (var (cat, a) in agg)
        {
            summary[cat.ToString()] = new RiskSummaryDto(
                Count: a.Count,
                TotalValue: a.TotalValue,
                TopClient: a.TopClient
            );
        }

        return (categories, summary);
    }
}