using FluentAssertions;
using RiskClassification.Application.Services;
using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;
using RiskClassification.Domain.Rules;

namespace RiskClassification.UnitTests;

public class RiskAnalyzerSummaryTests
{
    private readonly RiskAnalyzer _analyzer;

    public RiskAnalyzerSummaryTests()
    {
        var rules = new IRiskRule[] { new LowRiskRule(), new MediumRiskRule(), new HighRiskRule() };
        var classifier = new RiskClassifier(rules);
        _analyzer = new RiskAnalyzer(classifier);
    }

    [Fact]
    public void Should_compute_summary_and_top_client_per_category()
    {
        var trades = new List<Trade>
        {
            new(400_000m,  ClientSector.Public,  "CLI001"), // LOW
            new(500_000m,  ClientSector.Public,  "CLI001"), // LOW -> top CLI001 (900k)
            new(3_000_000m,ClientSector.Public,  "CLI002"), // MEDIUM -> top CLI002
            new(2_000_000m,ClientSector.Private, "CLI003"), // HIGH
            new(2_500_000m,ClientSector.Private, "CLI003"), // HIGH -> top CLI003 (4.5M)
        };

        var (categories, summary) = _analyzer.Analyze(trades);

        categories.Should().Equal("LOWRISK", "LOWRISK", "MEDIUMRISK", "HIGHRISK", "HIGHRISK");

        summary["LOWRISK"].Count.Should().Be(2);
        summary["LOWRISK"].TotalValue.Should().Be(900_000m);
        summary["LOWRISK"].TopClient.Should().Be("CLI001");

        summary["MEDIUMRISK"].Count.Should().Be(1);
        summary["MEDIUMRISK"].TotalValue.Should().Be(3_000_000m);
        summary["MEDIUMRISK"].TopClient.Should().Be("CLI002");

        summary["HIGHRISK"].Count.Should().Be(2);
        summary["HIGHRISK"].TotalValue.Should().Be(4_500_000m);
        summary["HIGHRISK"].TopClient.Should().Be("CLI003");
    }
}