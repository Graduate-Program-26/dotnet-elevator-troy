namespace domain.interfaces;

public interface IDispatchStrategy
{
    IElevator SelectElevator(IReadOnlyList<IElevator> candidates, IFloor targetFloor);
}