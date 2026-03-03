namespace RiskClassification.Application.DTOs;

public sealed record AnalyzeResponse(
    List<string> Categories,
    Dictionary<string, RiskSummaryDto> Summary,
    long ProcessingTimeMs
);

public sealed record RiskSummaryDto(
    int Count,
    decimal TotalValue,
    string TopClient
);