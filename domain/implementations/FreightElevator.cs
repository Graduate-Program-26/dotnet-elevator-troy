using domain.classes;

namespace domain.implementations;

using domain.interfaces;


public class FreightElevator : ElevatorBase
{   
    public FreightElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
        //stubbed I probably wont have time to fully flesh out the different elevator types
    }
}