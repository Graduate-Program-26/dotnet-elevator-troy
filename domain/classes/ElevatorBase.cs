using domain.enums;
using domain.exceptions;
using domain.interfaces;

namespace domain.classes;

/// <summary>Provides the core behaviour shared by all elevator types.</summary>
public abstract class ElevatorBase : IElevator
{
    private readonly List<IPassenger> _boardedPassengers = new();

    /// <summary>Initialises the elevator with a capacity and starting floor.</summary>
    /// <param name="capacity">The maximum number of passengers the elevator can carry.</param>
    /// <param name="startingFloor">The floor the elevator begins on.</param>
    protected ElevatorBase(int capacity, IFloor startingFloor)
    {
        Capacity = capacity;
        CurrentFloor = startingFloor;
        WishDirection = Direction.None;
    }

    public IReadOnlyList<IPassenger> BoardedPassengers => _boardedPassengers;
    public int Capacity { get; }
    public IFloor CurrentFloor { get; private set; }
    public Direction WishDirection { get; private set; }

    /// <summary>
    ///     This is how I've chosen to implement the highspeed elevator, we have floorspertick = 1 by default then we can
    ///     override in child
    /// </summary>
    public int FloorsPerTick { get; protected init; } = 1;

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
        {
            throw new ElevatorAtCapacityException(Capacity);
        }

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