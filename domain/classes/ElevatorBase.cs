namespace domain.classes;

using domain.enums;
using domain.exceptions;
using domain.interfaces;

public abstract class ElevatorBase : IElevator
{
    private readonly List<IPassenger> _boardedPassengers = new();

    public IReadOnlyList<IPassenger> BoardedPassengers => _boardedPassengers;
    public int Capacity { get; }
    public IFloor CurrentFloor { get; private set; }
    public Direction WishDirection { get; private set; }

    protected ElevatorBase(int capacity, IFloor startingFloor)
    {
        Capacity = capacity;
        CurrentFloor = startingFloor;
        WishDirection = Direction.None;
    }

    public virtual void MoveUp(IFloor nextFloor)
    {
        CurrentFloor = nextFloor;
    }

    public virtual void MoveDown(IFloor nextFloor)
    {
        CurrentFloor = nextFloor;
    }

    public void Board(IPassenger passenger)
    {
        if (_boardedPassengers.Count >= Capacity)
            throw new ElevatorAtCapacityException(Capacity);
        _boardedPassengers.Add(passenger);
    }

    public void Deboard(IPassenger passenger)
    {
        _boardedPassengers.Remove(passenger);
    }

    public void SetDirection(Direction direction)
    {
        WishDirection = direction;
    }
}