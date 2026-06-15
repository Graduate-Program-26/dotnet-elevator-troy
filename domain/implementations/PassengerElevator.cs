namespace domain.implementations;

using domain.enums;
using domain.interfaces;

public class PassengerElevator : IElevator
{
    public List<IPassenger> BoardedPassengers { get; set; }
    public int Capacity { get; set; }
    public IFloor CurrentFloor { get; set; }
    public Direction WishDirection { get; set; }

    private readonly List<IFloor> _floors;

    public PassengerElevator(int capacity, IFloor startingFloor, List<IFloor> floors)
    {
        Capacity = capacity;
        CurrentFloor = startingFloor;
        BoardedPassengers = new List<IPassenger>();
        WishDirection = default;
        _floors = floors;
    }

    public void MoveUp()
    {
        var nextFloor = _floors.FirstOrDefault(f => f.FloorNumber == CurrentFloor.FloorNumber + 1);
        if (nextFloor != null)
            CurrentFloor = nextFloor;
    }

    public void MoveDown()
    {
        var nextFloor = _floors.FirstOrDefault(f => f.FloorNumber == CurrentFloor.FloorNumber - 1);
        if (nextFloor != null)
            CurrentFloor = nextFloor;
    }
}