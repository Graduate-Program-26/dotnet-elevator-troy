using domain.interfaces;

namespace domain.implementations;

public class Floor : IFloor
{
    public Floor(int floorNumber, List<IPassenger> initialWaitingPassengers)
    {
        WaitingPassengers = initialWaitingPassengers;
        FloorNumber = floorNumber;
    }

    public int FloorNumber { get; }
    public List<IPassenger> WaitingPassengers { get; set; }
}