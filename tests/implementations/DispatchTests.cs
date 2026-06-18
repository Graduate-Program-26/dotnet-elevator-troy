using application.strategies;

using domain.implementations;
using domain.interfaces;

using Moq;

namespace tests.implementations;

public class DispatchTests
{
    private static IFloor CreateFloor(int floorNumber)
    {
        Mock<IFloor> mock = new();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock.Object;
    }

    [Fact]
    public void Dispatch_SelectsNearestAvailableElevator()
    {
        NearestFloorStrategy strategy = new();
        IFloor target = CreateFloor(5);
        PassengerElevator near = new(10, CreateFloor(4));
        PassengerElevator far = new(10, CreateFloor(1));

        IElevator result = strategy.SelectElevator(new List<IElevator> { far, near }, target);

        Assert.Equal(near, result);
    }

    [Fact]
    public void Dispatch_SelectsNearestElevator_WhenAboveTarget()
    {
        NearestFloorStrategy strategy = new();
        IFloor target = CreateFloor(3);
        PassengerElevator near = new(10, CreateFloor(4));
        PassengerElevator far = new(10, CreateFloor(10));

        IElevator result = strategy.SelectElevator(new List<IElevator> { far, near }, target);

        Assert.Equal(near, result);
    }
}