using domain.interfaces;

namespace domain.implementations;

public class Floor : IFloor
{
    public int FloorNumber { get; }
    
    public Floor(int floorNumber)
    {
        FloorNumber = floorNumber;
    }
}