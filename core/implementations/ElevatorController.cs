namespace core.implementations;

using domain.enums;
using domain.interfaces;

public class ElevatorController
{
    private readonly List<IFloor> _floors;
    private readonly List<IElevator> _elevators;
    private readonly IDispatchStrategy _dispatchStrategy;

    public ElevatorController(List<IFloor> floors, List<IElevator> elevators, IDispatchStrategy dispatchStrategy)
    {
        _floors = floors;
        _elevators = elevators;
        _dispatchStrategy = dispatchStrategy;
    }

    public IElevator Dispatch(IFloor targetFloor)
    {
        var elevator = _dispatchStrategy.SelectElevator(_elevators, targetFloor);
        MoveToFloor(elevator, targetFloor);
        return elevator;
    }

    public void MoveToFloor(IElevator elevator, IFloor targetFloor)
    {
        while (elevator.CurrentFloor.FloorNumber != targetFloor.FloorNumber)
        {
            if (elevator.CurrentFloor.FloorNumber < targetFloor.FloorNumber)
            {
                elevator.SetDirection(Direction.Upwards);
                var next = _floors.First(f => f.FloorNumber == elevator.CurrentFloor.FloorNumber + 1);
                elevator.MoveUp(next);
            }
            else
            {
                elevator.SetDirection(Direction.Downwards);
                var next = _floors.First(f => f.FloorNumber == elevator.CurrentFloor.FloorNumber - 1);
                elevator.MoveDown(next);
            }
        }

        elevator.SetDirection(Direction.None);
    }

    public IFloor GetFloor(int floorNumber)
    {
        return _floors.First(f => f.FloorNumber == floorNumber);
    }
}