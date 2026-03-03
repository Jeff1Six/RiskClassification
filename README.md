# Risk Classification API


![.NET CI](https://github.com/Jeff1Six/RiskClassification/actions/workflows/dotnet.yml/badge.svg)

## 📌 Overview

This project implements a REST API in **ASP.NET Core 8** for automatic
classification of financial trades according to risk level.

The system processes up to **100,000 trades per request** and provides
both individual risk classification and statistical portfolio analysis.

The design focuses on:

-   Clean Architecture principles
-   Extensibility of business rules
-   Performance (O(n) processing)
-   Separation of concerns
-   Automated testing

------------------------------------------------------------------------

## 🏗 Architecture

The solution is organized into the following layers:

    src/
     ├── RiskClassification.Domain
     ├── RiskClassification.Application
     ├── RiskClassification.API

    tests/
     ├── RiskClassification.UnitTests
     ├── RiskClassification.IntegrationTests

### 🔹 Domain

Contains core business rules and entities: - Trade entity - RiskCategory
enum - ClientSector enum - IRiskRule interface - Risk rule
implementations (Strategy Pattern)

### 🔹 Application

Contains: - DTOs - RiskClassifier service - RiskAnalyzer service -
Mapping and validation logic

### 🔹 API

Responsible for: - Controllers - Middleware - Dependency injection -
Logging - Swagger documentation

------------------------------------------------------------------------

## 🧠 Design Decisions

### ✔ Strategy Pattern for Risk Rules

Risk classification rules are implemented using the **Strategy
Pattern**, allowing new risk categories to be added without modifying
existing logic.

To add a new rule: 1. Implement `IRiskRule` 2. Register it in dependency
injection

No core logic modification required.

------------------------------------------------------------------------

### ✔ Performance Optimization

The analysis endpoint processes trades in **O(n)** time complexity.

-   Single-pass aggregation
-   Dictionary-based exposure tracking
-   Real-time top client calculation (no sorting required)

This ensures scalability for up to 100,000 trades per request.

------------------------------------------------------------------------

### ✔ Logging

-   Request logging middleware
-   Global exception handling middleware
-   Structured logging via `ILogger`

------------------------------------------------------------------------

### ✔ Validation & Error Handling

-   Input validation
-   Business rule validation
-   Centralized exception handling middleware
-   Proper HTTP status codes (400 / 500)

------------------------------------------------------------------------

## 🚀 Endpoints

### POST `/api/trades/classify`

Classifies trades by risk.

#### Request

``` json
{
  "trades": [
    { "value": 2000000, "clientSector": "Private", "clientId": "CLI003" },
    { "value": 400000,  "clientSector": "Public",  "clientId": "CLI001" }
  ]
}
```

#### Response

``` json
{
  "categories": ["HIGHRISK", "LOWRISK"]
}
```

------------------------------------------------------------------------

### POST `/api/trades/analyze`

Returns classification + statistical summary.

#### Response Example

``` json
{
  "categories": ["HIGHRISK", "LOWRISK"],
  "summary": {
    "LOWRISK": {
      "count": 1,
      "totalValue": 400000,
      "topClient": "CLI001"
    },
    "HIGHRISK": {
      "count": 1,
      "totalValue": 2000000,
      "topClient": "CLI003"
    }
  },
  "processingTimeMs": 12
}
```

------------------------------------------------------------------------

## 🧪 Running the Project

### Run API

    dotnet run --project src/RiskClassification.API

Swagger will be available at:

    https://localhost:<port>/swagger

------------------------------------------------------------------------

### Run Tests

    dotnet test

Includes: - Unit tests for business rules - Integration tests for API
endpoints - Performance test for 100k trades scenario

------------------------------------------------------------------------

## 📈 Scalability Considerations

-   Stateless API
-   In-memory processing
-   O(n) algorithmic complexity
-   Easily extensible rule system

The architecture allows easy addition of: - Persistence layer
(Infrastructure) - Caching - Audit logging - Additional risk categories

------------------------------------------------------------------------

## 👤 Author

Developed as a technical challenge demonstrating:

-   .NET 8 proficiency
-   Clean Architecture
-   Design patterns
-   Performance awareness
-   API design best practices
