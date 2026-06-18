using domain.classes;
using domain.interfaces;

namespace domain.implementations;

public class HighSpeedElevator : ElevatorBase
{
    private const int SpeedMultiplier = 2;

    public HighSpeedElevator(int capacity, IFloor startingFloor)
        : base(capacity, startingFloor)
    {
        FloorsPerTick = SpeedMultiplier;
    }
}