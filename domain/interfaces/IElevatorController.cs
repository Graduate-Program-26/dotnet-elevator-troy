namespace domain.interfaces;

public interface IElevatorController
{
    IElevator SelectElevator(IFloor targetFloor, IReadOnlyList<IElevator> candidates);
    void MoveToFloor(IElevator elevator, IFloor targetFloor);
    IFloor GetFloor(int floorNumber);
}