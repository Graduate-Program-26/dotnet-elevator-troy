using application.implementations;
using application.strategies;
using domain.implementations;
using domain.interfaces;
using display.implementations;

using domain.exceptions;
using Serilog;


const int elevatorCapacity = 8;
const int tickDelayMs = 500;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"logs/simulation_{DateTime.Now:yyyyMMdd_HHmmss}.log")
    .CreateLogger();

static int ReadInt(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value) && value >= min && value <= max)
            return value;
        Console.WriteLine($"Enter a number between {min} and {max}.");
    }
}

var floorCount = ReadInt("Floors: ",     ElevatorController.MinFloorCount,    ElevatorController.MaxFloorCount);
var elevatorCount  = ReadInt("Elevators: ",  ElevatorController.MinElevatorCount, ElevatorController.MaxElevatorCount);
var passengerCount = ReadInt("Passengers: ", Simulation.MinPassengerCount,        Simulation.MaxPassengerCount);

var random = new Random();

var floors = Enumerable.Range(1, floorCount)
    .Select(n => (IFloor)new Floor(n, new List<IPassenger>()))
    .ToList();

IFloor StartingFloor(int elevatorIndex)
{
    if (elevatorCount == 1)
        return floors[0];

    var index = elevatorIndex * (floorCount - 1) / (elevatorCount - 1);
    return floors[index];
}

var elevators = Enumerable.Range(0, elevatorCount)
    .Select(i => (IElevator)new PassengerElevator(elevatorCapacity, StartingFloor(i)))
    .ToList();

var strategy = new NearestFloorStrategy();
var controller = new ElevatorController(floors, elevators, strategy);
var simulation = new Simulation(floors, elevators, controller, passengerCount, random, Log.Logger);
var renderer = new ConsoleRenderer(floors, elevators);

try
{
    while (!simulation.IsComplete)
    {
        simulation.Tick();
        renderer.Render();
        Thread.Sleep(tickDelayMs);
    }

}
catch (ElevatorAtCapacityException ex)
{
    Console.WriteLine(ex.Message);
}
catch (FloorOutOfBoundsException ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine($"Simulation complete. {simulation.Delivered} passengers delivered.");