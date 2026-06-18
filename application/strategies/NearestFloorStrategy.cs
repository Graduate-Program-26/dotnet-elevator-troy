using domain.interfaces;

namespace application.strategies;

public class NearestFloorStrategy : IDispatchStrategy
{
    public IElevator SelectElevator(IReadOnlyList<IElevator> candidates, IFloor targetFloor)
    {
        return candidates.OrderBy(e => Math.Abs(e.CurrentFloor.FloorNumber - targetFloor.FloorNumber)).First();
    }
}