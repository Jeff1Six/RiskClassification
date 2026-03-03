namespace RiskClassification.Application.DTOs;

public sealed record ClassifyRequest(List<TradeInputDto> Trades);