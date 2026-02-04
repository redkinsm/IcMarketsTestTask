# ICMarkets – Web API .NET Developer Project

Greetings from the ICMarkets team.

The purpose of this project is to validate the candidate's approach to designing a Web API.

---

## Project Overview

The application stores blockchain data retrieved from the following BlockCypher API endpoints:

1. https://api.blockcypher.com/v1/eth/main
2. https://api.blockcypher.com/v1/dash/main
3. https://api.blockcypher.com/v1/btc/main
4. https://api.blockcypher.com/v1/btc/test3
5. https://api.blockcypher.com/v1/ltc/main

**Documentation:**  
Blockchain API – Blockchain Developer API for Bitcoin, Ethereum, Testnet, Litecoin and More | BlockCypher

---

## Minimum Functional Requirements

1. .NET Core application based on **Clean Architecture** or **Vertical Slice Architecture**, following **SOLID principles**
2. API endpoints exposed via **Swagger**
    - Endpoints must show the **history of each blockchain’s data** stored in the database
3. Store blockchain data in the database with an additional timestamp:
    - `CreatedAt` – time when the API request was made
    - Data history must be sorted by `CreatedAt` in **descending order**
4. Implement:
    - HealthChecks endpoint
    - Basic CORS policy
5. Use:
    - Dependency Injection
    - Logging
    - Model mapping
    - API serialization
    - Automatic behavior validation
6. Provide separate projects for:
    - Unit tests
    - Integration tests
    - Functional tests
7. Runtime profiles:
    - .NET
    - Docker (Linux)

---

## Framework & Technical Requirements

1. .NET Core (preferably **.NET 6 or higher**)
2. Database options:
    - SQLite (Entity Framework)
    - Database in Docker
    - NoSQL (optional)
3. Main data must be stored as provided in the API’s JSON responses
4. Application should illustrate **best practices**:
    - Performance
    - Inheritance
    - Scalability
5. Use **asynchronous or parallel patterns**
    - Tasks
    - PLINQ
    - etc.
6. Provide **at least two design patterns**, such as:
    - Unit of Work
    - Repository
    - CQRS
    - Event Sourcing
7. Optional:
    - API Gateway

---

## Submission Requirements

- Public GitHub repository
- `README.md` with setup and run instructions
- Repository must contain:
    - `main` branch
    - `development` branch

The solution should be submitted via email by providing the public GitHub URL.