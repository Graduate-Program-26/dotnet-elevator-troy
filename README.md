# Elevator Simulation

C# Dotnet project which simulates elevators in a building, the elevators make use of the strategy design pattern to swap the dispatching algorithm. The simulation runs until all passengers have gotten off at their desired floor.

## Prerequisites

- .NET 10 SDK

## Setup

```
git clone git@github.com:Graduate-Program-26/dotnet-elevator-troy.git
cd dotnet-elevator-troy
dotnet restore
dotnet build
```

## Running

```
dotnet run
```

The app then prompts you to enter values:

- Floors: 2 to 99
- Elevators: 1 to 9
- Passengers: 1 to 100

Elevator data is displayed, which shows the current floor, the direction of the elevator and the current capacity of passengers.

## Tests

```
dotnet test
```

## Project Structure

This project implements clean architecture.

- `domain` 
- `application`
- `display` 
- `domain.tests` 

