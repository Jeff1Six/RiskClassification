using FluentAssertions;
using RiskClassification.Application.Services;
using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;
using RiskClassification.Domain.Rules;

namespace RiskClassification.UnitTests;

public class RiskClassifierTests
{
    private readonly RiskClassifier _sut = new(new IRiskRule[]
    {
        new LowRiskRule(),
        new MediumRiskRule(),
        new HighRiskRule()
    });

    [Fact]
    public void Should_classify_lowrisk_when_value_below_1m()
    {
        var trade = new Trade(400_000m, ClientSector.Public, "CLI001");
        _sut.Classify(trade).Should().Be(RiskCategory.LOWRISK);
    }

    [Fact]
    public void Should_classify_mediumrisk_when_public_and_value_ge_1m()
    {
        var trade = new Trade(3_000_000m, ClientSector.Public, "CLI002");
        _sut.Classify(trade).Should().Be(RiskCategory.MEDIUMRISK);
    }

    [Fact]
    public void Should_classify_highrisk_when_private_and_value_ge_1m()
    {
        var trade = new Trade(2_000_000m, ClientSector.Private, "CLI003");
        _sut.Classify(trade).Should().Be(RiskCategory.HIGHRISK);
    }

}