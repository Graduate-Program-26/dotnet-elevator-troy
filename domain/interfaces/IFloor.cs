namespace domain.interfaces;

public interface IFloor
{
    int FloorNumber { get; }
    List<IPassenger> WaitingPassengers { get; set; }
}