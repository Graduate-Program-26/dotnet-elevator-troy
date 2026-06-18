using domain.interfaces;

namespace display.implementations;

public class ConsoleRenderer
{
    private readonly List<IElevator> _elevators;
    private readonly List<IFloor> _floors;
    private readonly int _passengerSlots;

    public ConsoleRenderer(List<IFloor> floors, List<IElevator> elevators, int passengerSlots = 5)
    {
        _floors = floors;
        _elevators = elevators;
        _passengerSlots = passengerSlots;
    }

    public void Render()
    {
        Console.Clear();

        foreach (IFloor floor in _floors.OrderByDescending(f => f.FloorNumber))
        {
            string elevatorSection = string.Join(" ", _elevators
                .Select(e => e.CurrentFloor.FloorNumber == floor.FloorNumber ? '#' : ' '));

            int waitingCount = floor.WaitingPassengers.Count;
            string passengerSection = string.Join(" ", Enumerable.Range(0, _passengerSlots)
                .Select(i => i < waitingCount ? 'o' : ' '));

            Console.WriteLine($"Floor {floor.FloorNumber,2} | {elevatorSection} | {passengerSection}");
        }

        Console.WriteLine();

        for (int i = 0; i < _elevators.Count; i++)
        {
            IElevator elevator = _elevators[i];
            //cool thing I learned about c# console output, the integer arg allows us to pad the output so the | doesn't shift everytime direction changes, it will always be left-aligned 10 chars.
            Console.WriteLine(
                $"Elevator {i + 1} | Floor {elevator.CurrentFloor.FloorNumber,2} | Direction: {elevator.WishDirection,-10} | Passengers: {elevator.BoardedPassengers.Count}/{elevator.Capacity}");
        }
    }
}