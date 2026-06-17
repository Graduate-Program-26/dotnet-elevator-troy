namespace core.strategies;
using domain.interfaces;

public class NearestFloorStrategy : IDispatchStrategy
{
    public IElevator SelectElevator(IReadOnlyList<IElevator> candidates, IFloor targetFloor) =>
        candidates.OrderBy(e => Math.Abs(e.CurrentFloor.FloorNumber - targetFloor.FloorNumber)).First();

}