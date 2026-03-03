using System.Diagnostics;
using FluentAssertions;
using RiskClassification.Application.Services;
using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;
using RiskClassification.Domain.Rules;

namespace RiskClassification.UnitTests;

public class RiskAnalyzerTests
{
    private readonly RiskAnalyzer _analyzer;

    public RiskAnalyzerTests()
    {
        var rules = new IRiskRule[]
        {
            new LowRiskRule(),
            new MediumRiskRule(),
            new HighRiskRule()
        };

        var classifier = new RiskClassifier(rules);
        _analyzer = new RiskAnalyzer(classifier);
    }

    [Fact]
    public void Should_process_100k_trades_under_reasonable_time()
    {
        var trades = Enumerable.Range(1, 100_000)
            .Select(i => new Trade(1_500_000m, ClientSector.Private, $"CLI{i % 10}"))
            .ToList();

        var sw = Stopwatch.StartNew();

        _analyzer.Analyze(trades);

        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(1000);
    }
}