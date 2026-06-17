using core.implementations;
using domain.implementations;
using domain.interfaces;
using display.implementations;

const int ElevatorCapacity = 8;
const int TickDelayMs = 500;

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

var floorCount    = ReadInt("Floors: ",     ElevatorController.MinFloorCount,    ElevatorController.MaxFloorCount);
var elevatorCount = ReadInt("Elevators: ",  ElevatorController.MinElevatorCount, ElevatorController.MaxElevatorCount);
var passengerCount = ReadInt("Passengers: ", Simulation.MinPassengerCount,       Simulation.MaxPassengerCount);

var random = new Random();

var floors = Enumerable.Range(1, floorCount)
    .Select(number => (IFloor)new Floor(number, new List<IPassenger>()))
    .ToList();

IFloor StartingFloor(int elevatorIndex)
{
    if (elevatorCount == 1)
        return floors[0];

    var index = elevatorIndex * (floorCount - 1) / (elevatorCount - 1);
    return floors[index];
}

var elevators = Enumerable.Range(0, elevatorCount)
    .Select(i => (IElevator)new PassengerElevator(ElevatorCapacity, StartingFloor(i)))
    .ToList();

var simulation = new Simulation(floors, elevators, passengerCount, random);
var renderer = new ConsoleRenderer(floors, elevators);

while (!simulation.IsComplete)
{
    simulation.Tick();
    renderer.Render();
    Thread.Sleep(TickDelayMs);
}

Console.WriteLine($"Simulation complete. {simulation.Delivered} passengers delivered.");