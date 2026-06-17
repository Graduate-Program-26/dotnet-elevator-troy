namespace core.strategies;
using domain.interfaces;

public class NearestFloorStrategy : IDispatchStrategy
{
    public IElevator SelectElevator(List<IElevator> elevators, IFloor targetFloor)
    {   
        return elevators
            .OrderBy(e => Math.Abs(e.CurrentFloor.FloorNumber - targetFloor.FloorNumber))
            .First();
    }
}