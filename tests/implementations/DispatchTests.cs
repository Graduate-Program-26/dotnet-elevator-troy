namespace tests.implementations;

using application.strategies;
using domain.implementations;
using domain.interfaces;
using Moq;

public class DispatchTests
{
    private static IFloor CreateFloor(int floorNumber)
    {
        var mock = new Mock<IFloor>();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock.Object;
    }

    [Fact]
    public void Dispatch_SelectsNearestAvailableElevator()
    {
        var strategy = new NearestFloorStrategy();
        var target = CreateFloor(5);
        var near = new PassengerElevator(10, CreateFloor(4));
        var far = new PassengerElevator(10, CreateFloor(1));

        var result = strategy.SelectElevator(new List<IElevator> { far, near }, target);

        Assert.Equal(near, result);
    }

    [Fact]
    public void Dispatch_SelectsNearestElevator_WhenAboveTarget()
    {
        var strategy = new NearestFloorStrategy();
        var target = CreateFloor(3);
        var near = new PassengerElevator(10, CreateFloor(4));
        var far = new PassengerElevator(10, CreateFloor(10));

        var result = strategy.SelectElevator(new List<IElevator> { far, near }, target);

        Assert.Equal(near, result);
    }
}