using domain.interfaces;

namespace domain.implementations;

public class Floor : IFloor
{
    public int FloorNumber { get; }
    public List<IPassenger> WaitingPassengers { get; set; }

    public Floor(int floorNumber, List<IPassenger> initialWaitingPassengers)  
    {   
        WaitingPassengers = initialWaitingPassengers;
        FloorNumber = floorNumber;
    }
}