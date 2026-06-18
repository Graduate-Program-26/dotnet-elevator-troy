using domain.classes;
using domain.interfaces;

namespace domain.implementations;

public class PassengerElevator : ElevatorBase
{
    public PassengerElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
    }
}