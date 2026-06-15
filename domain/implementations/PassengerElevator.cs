namespace domain.implementations;

using domain.enums;
using domain.exceptions;
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
        var nextFloorNumber = CurrentFloor.FloorNumber + 1;
        var nextFloor = _floors.FirstOrDefault(f => f.FloorNumber == nextFloorNumber);
        if (nextFloor == null)
            throw new FloorOutOfBoundsException(nextFloorNumber);
        CurrentFloor = nextFloor;
    }

    public void MoveDown()
    {
        var nextFloorNumber = CurrentFloor.FloorNumber - 1;
        var nextFloor = _floors.FirstOrDefault(f => f.FloorNumber == nextFloorNumber);
        if (nextFloor == null)
            throw new FloorOutOfBoundsException(nextFloorNumber);
        CurrentFloor = nextFloor;
    }

    public void Board(IPassenger passenger)
    {
        if (BoardedPassengers.Count >= Capacity)
            throw new ElevatorAtCapacityException(Capacity);
        BoardedPassengers.Add(passenger);
    }

    public void Deboard(IPassenger passenger)
    {
        BoardedPassengers.Remove(passenger);
    }

    public void SetDirection(Direction direction)
    {
        var topFloor = _floors.Max(f => f.FloorNumber);
        var bottomFloor = _floors.Min(f => f.FloorNumber);

        if (direction == Direction.Upwards && CurrentFloor.FloorNumber == topFloor)
            throw new InvalidDirectionException(CurrentFloor.FloorNumber, topFloor);

        if (direction == Direction.Downwards && CurrentFloor.FloorNumber == bottomFloor)
            throw new InvalidDirectionException(CurrentFloor.FloorNumber, bottomFloor);

        WishDirection = direction;
    }
}