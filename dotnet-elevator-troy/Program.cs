using application.implementations;
using application.strategies;

using display.implementations;

using domain.exceptions;
using domain.implementations;
using domain.interfaces;

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
        if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
        {
            return value;
        }

        Console.WriteLine($"Enter a number between {min} and {max}.");
    }
}

int floorCount = ReadInt("Floors: ", ElevatorController.MinFloorCount, ElevatorController.MaxFloorCount);
int normalELevatorCount = ReadInt("Regular elevators: ", ElevatorController.MinElevatorCount,
    ElevatorController.MaxElevatorCount);
int highSpeedCount = ReadInt("High-speed elevators: ", 0, ElevatorController.MaxElevatorCount - normalELevatorCount);
int elevatorCount = normalELevatorCount + highSpeedCount;
int passengerCount = ReadInt("Passengers: ", Simulation.MinPassengerCount, Simulation.MaxPassengerCount);

Random random = new();

List<IFloor> floors = Enumerable.Range(1, floorCount)
    .Select(n => (IFloor)new Floor(n, new List<IPassenger>()))
    .ToList();

IFloor StartingFloor(int elevatorIndex, int total)
{
    if (total == 1)
    {
        return floors[0];
    }

    int index = elevatorIndex * (floorCount - 1) / (total - 1);
    return floors[index];
}

IEnumerable<IElevator> regularElevators = Enumerable.Range(0, normalELevatorCount)
    .Select(i => (IElevator)new PassengerElevator(elevatorCapacity, StartingFloor(i, elevatorCount)));

IEnumerable<IElevator> highSpeedElevators = Enumerable.Range(0, highSpeedCount)
    .Select(i =>
        (IElevator)new HighSpeedElevator(elevatorCapacity, StartingFloor(normalELevatorCount + i, elevatorCount)));


List<IElevator> elevators = regularElevators.Concat(highSpeedElevators).ToList();

NearestFloorStrategy strategy = new();
ElevatorController controller = new(floors, elevators, strategy);
Simulation simulation = new(floors, elevators, controller, passengerCount, random, Log.Logger);
ConsoleRenderer renderer = new(floors, elevators);

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