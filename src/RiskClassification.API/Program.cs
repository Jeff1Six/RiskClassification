using RiskClassification.API.Middleware;
using RiskClassification.Application.Services;
using RiskClassification.Domain.Rules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRiskRule, LowRiskRule>();
builder.Services.AddSingleton<IRiskRule, MediumRiskRule>();
builder.Services.AddSingleton<IRiskRule, HighRiskRule>();

builder.Services.AddSingleton<RiskClassifier>();
builder.Services.AddSingleton<RiskAnalyzer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();
app.Run();

public partial class Program { }