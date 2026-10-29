# BookingRules for .NET

**A .NET 9 implementation of the Delphi BookingRules project, demonstrating
SOLID principles, dependency injection, consistent repository reads, and
automated testing with MSTest.**

This project intentionally implements the same reservation rules, decisions,
console examples, and 19 test scenarios as the Delphi 10.4 version.

## Features

- Reservation confirmation based on customer status and participant count.
- Detailed decisions explaining why a reservation was refused.
- Dependency injection through `ICustomerRepository`.
- Atomic customer-status lookup through a single `TryGetStatus` call.
- In-memory repository implementation.
- Unit tests using Fake, Spy, and failure-simulating repositories.
- Integration tests combining the domain with the in-memory repository.
- Console application demonstrating valid and invalid reservations.

## Business Rules

A reservation can be confirmed only when:

1. The participant count is between 1 and 5.
2. The customer exists and is active.
3. The customer is not blocked.

Repository exceptions are propagated rather than silently ignored.

`Reservation.Evaluate` returns a `ReservationDecision`. For callers that need
only a Boolean answer, `Reservation.CanConfirm` remains available.

| Decision | Meaning |
|---|---|
| `Confirmed` | All reservation rules were satisfied. |
| `InvalidParticipantCount` | The participant count is outside the accepted range. |
| `RepositoryUnavailable` | No customer repository was supplied. |
| `CustomerNotFound` | The repository could not find the customer. |
| `CustomerInactive` | The customer exists but is inactive. |
| `CustomerBlocked` | The customer is active but blocked. |

## Architecture

```text
BookingRules/
├── src/
│   ├── BookingRules.Domain/
│   ├── BookingRules.Infrastructure/
│   └── BookingRules.Console/
├── tests/
│   ├── BookingRules.UnitTests/
│   └── BookingRules.IntegrationTests/
├── BookingRules.sln
├── Directory.Build.props
└── global.json
```

- `BookingRules.Domain` contains the business rules and repository contract.
- `BookingRules.Infrastructure` contains the in-memory repository.
- `BookingRules.Console` is the executable demonstration.
- Unit and integration tests are kept in separate projects.

All projects enable nullable reference types and treat compiler warnings as
errors.

## Requirements

- .NET SDK 9.0.317 or a compatible .NET 9 SDK
- Windows, Linux, or macOS

The repository includes `global.json` to keep SDK selection reproducible.

## Build and Run

Restore and build the complete solution:

```powershell
dotnet restore BookingRules.sln
dotnet build BookingRules.sln --configuration Debug
```

Run the console demonstration:

```powershell
dotnet run --project src/BookingRules.Console/BookingRules.Console.csproj
```

Run all tests:

```powershell
dotnet test BookingRules.sln
```

## Automated Testing

The tests use MSTest and mirror the behaviors covered by the Delphi DUnitX
suite.

**Last verified execution:** October 29, 2026, using .NET SDK 9.0.317.

| Test project | Passed | Failed | Skipped |
|---|---:|---:|---:|
| Unit tests | 13 | 0 | 0 |
| Integration tests | 6 | 0 | 0 |
| **Total** | **19** | **0** | **0** |

## Delphi Equivalence

| Delphi | C# / .NET |
|---|---|
| `TReservation` | `Reservation` |
| `TReservationDecision` | `ReservationDecision` |
| `TCustomerStatus` | `CustomerStatus` |
| `ICustomerRepository` | `ICustomerRepository` |
| `TInMemoryCustomerRepository` | `InMemoryCustomerRepository` |
| DUnitX | MSTest |

## Scope

BookingRules is an educational and portfolio demonstration, not a production
booking platform. It focuses on business rules, object-oriented architecture,
cross-language equivalence, and automated tests.

## Author

**Marcelo Gutierrez**

[GitHub](https://github.com/mgmsystem)
