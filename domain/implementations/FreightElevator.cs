using domain.classes;

namespace domain.implementations;

using domain.interfaces;


public class FreightElevator : ElevatorBase
{   
    public FreightElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
    }
}