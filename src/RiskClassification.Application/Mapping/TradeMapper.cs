using RiskClassification.Application.DTOs;
using RiskClassification.Domain.Entities;
using RiskClassification.Domain.Enums;

namespace RiskClassification.Application.Mapping;

public static class TradeMapper
{
    public static Trade ToDomain(TradeInputDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ClientId))
            throw new ArgumentException("clientId is required");

        if (!Enum.TryParse<ClientSector>(dto.ClientSector, ignoreCase: true, out var sector))
            throw new ArgumentException("clientSector must be 'Public' or 'Private'");

        if (dto.Value < 0)
            throw new ArgumentException("value must be >= 0");

        return new Trade(dto.Value, sector, dto.ClientId);
    }
}