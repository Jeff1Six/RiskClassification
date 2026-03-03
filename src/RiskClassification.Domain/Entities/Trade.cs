using RiskClassification.Domain.Enums;

namespace RiskClassification.Domain.Entities;

public sealed class Trade
{
    public decimal Value { get; }
    public ClientSector ClientSector { get; }
    public string ClientId { get; }

    public Trade(decimal value, ClientSector clientSector, string clientId)
    {
        Value = value;
        ClientSector = clientSector;
        ClientId = clientId;
    }
}