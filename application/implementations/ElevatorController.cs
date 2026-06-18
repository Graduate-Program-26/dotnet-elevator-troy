using domain.exceptions;

namespace application.implementations;

using domain.enums;
using domain.interfaces;

public class ElevatorController : IElevatorController
{
    public const int MinFloorCount = 2;
    public const int MaxFloorCount = 99;
    public const int MinElevatorCount = 1;
    public const int MaxElevatorCount = 9;

    private readonly List<IFloor> _floors;
    private readonly IDispatchStrategy _dispatchStrategy;

    public ElevatorController(List<IFloor> floors, List<IElevator> elevators, IDispatchStrategy dispatchStrategy)
    {
        if (floors.Count > MaxFloorCount)
            throw new FloorOutOfBoundsException(floors.Count);

        if (elevators.Count > MaxElevatorCount)
            throw new TooManyElevatorsException(elevators.Count);

        _floors = floors;
        _dispatchStrategy = dispatchStrategy;
    }

    public IElevator Dispatch(IFloor targetFloor, IReadOnlyList<IElevator> candidates)
    {
        var elevator = _dispatchStrategy.SelectElevator(candidates, targetFloor);
        return elevator;
    }

    public void MoveToFloor(IElevator elevator, IFloor targetFloor)
    {
        var diff = targetFloor.FloorNumber - elevator.CurrentFloor.FloorNumber;
        if (diff > 0)
        {
            elevator.SetDirection(Direction.Upwards);
            elevator.MoveUp(GetFloor(elevator.CurrentFloor.FloorNumber + 1));
        }
        else if (diff < 0)
        {
            elevator.SetDirection(Direction.Downwards);
            elevator.MoveDown(GetFloor(elevator.CurrentFloor.FloorNumber - 1));
        }
    }

    public IFloor GetFloor(int floorNumber) =>
        _floors.First(f => f.FloorNumber == floorNumber);
}