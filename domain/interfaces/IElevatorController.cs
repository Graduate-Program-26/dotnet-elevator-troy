namespace domain.interfaces;

public interface IElevatorController
{
    IElevator Dispatch(IFloor targetFloor, IReadOnlyList<IElevator> candidateElevators);
    void MoveToFloor(IElevator elevator, IFloor targetFloor);
    IFloor GetFloor(int floorNumber);
}