using domain.classes;
using domain.interfaces;

namespace domain.implementations;

public class FreightElevator : ElevatorBase
{
    public FreightElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
    }
}