namespace RiskClassification.Application.DTOs;

public sealed record TradeInputDto(
    decimal Value,
    string ClientSector,
    string ClientId
);