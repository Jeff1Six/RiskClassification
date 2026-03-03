using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RiskClassification.Application.DTOs;

namespace RiskClassification.IntegrationTests;

public class TradesEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TradesEndpointsTests(WebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Classify_should_return_categories_in_order()
    {
        var req = new ClassifyRequest(new()
        {
            new TradeInputDto(2_000_000m, "Private", "CLI003"),
            new TradeInputDto(400_000m, "Public", "CLI001"),
            new TradeInputDto(500_000m, "Public", "CLI001"),
            new TradeInputDto(3_000_000m, "Public", "CLI002"),
        });

        var res = await _client.PostAsJsonAsync("/api/trades/classify", req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await res.Content.ReadFromJsonAsync<ClassifyResponse>();
        body!.Categories.Should().Equal("HIGHRISK", "LOWRISK", "LOWRISK", "MEDIUMRISK");
    }
}