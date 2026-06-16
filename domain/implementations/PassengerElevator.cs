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

    public PassengerElevator(int capacity, IFloor startingFloor)
    {
        Capacity = capacity;
        CurrentFloor = startingFloor;
        BoardedPassengers = new List<IPassenger>();
        WishDirection = Direction.None;
    }

    public void MoveUp(IFloor nextFloor)
    {
        CurrentFloor = nextFloor;
    }

    public void MoveDown(IFloor nextFloor)
    {
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
        WishDirection = direction;
    }
}