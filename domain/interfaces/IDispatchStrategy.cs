namespace domain.interfaces;

public interface IDispatchStrategy
{
    IElevator SelectElevator(List<IElevator> elevators, IFloor targetFloor);
}