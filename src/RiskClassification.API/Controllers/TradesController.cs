using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RiskClassification.Application.DTOs;
using RiskClassification.Application.Mapping;
using RiskClassification.Application.Services;

namespace RiskClassification.API.Controllers;

[ApiController]
[Route("api/trades")]
public sealed class TradesController : ControllerBase
{
    private readonly RiskClassifier _classifier;
    private readonly RiskAnalyzer _analyzer;
    private readonly ILogger<TradesController> _logger;

    public TradesController(RiskClassifier classifier, RiskAnalyzer analyzer, ILogger<TradesController> logger)
    {
        _classifier = classifier;
        _analyzer = analyzer;
        _logger = logger;
    }

    [HttpPost("classify")]
    public ActionResult<ClassifyResponse> Classify([FromBody] ClassifyRequest request)
    {
        if (request?.Trades is null || request.Trades.Count == 0)
            return BadRequest(new { error = "trades must not be empty" });

        var trades = request.Trades.Select(TradeMapper.ToDomain).ToList();

        var categories = trades
            .Select(t => _classifier.Classify(t).ToString())
            .ToList();
        _logger.LogInformation("Classify called with {Count} trades", request.Trades.Count);
        return Ok(new ClassifyResponse(categories));
    }

    [HttpPost("analyze")]
    public ActionResult<AnalyzeResponse> Analyze([FromBody] ClassifyRequest request)
    {
        _logger.LogInformation("Analyze called with {Count} trades", request.Trades.Count);

        if (request?.Trades is null || request.Trades.Count == 0)
            return BadRequest(new { error = "trades must not be empty" });

        if (request.Trades.Count > 100_000)
        {
            _logger.LogWarning("Analyze rejected: too many trades ({Count})", request.Trades.Count);
            return BadRequest(new { error = "Max trades per request is 100000" });
        }

        var sw = Stopwatch.StartNew();

        var trades = request.Trades.Select(TradeMapper.ToDomain).ToList();
        var (categories, summary) = _analyzer.Analyze(trades);

        sw.Stop();

        return Ok(new AnalyzeResponse(
            Categories: categories,
            Summary: summary,
            ProcessingTimeMs: sw.ElapsedMilliseconds
        ));
    }
}