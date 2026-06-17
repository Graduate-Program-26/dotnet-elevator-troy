using domain.classes;

namespace domain.implementations;

using domain.interfaces;


public class PassengerElevator : ElevatorBase
{
    public PassengerElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
    }
}