using application.implementations;

using domain.exceptions;
using domain.implementations;
using domain.interfaces;

using Moq;

namespace domain.tests.implementations;

public class ElevatorValidationTests
{
    private static IFloor CreateFloor(int floorNumber)
    {
        var mock = new Mock<IFloor>();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock.Object;
    }

    private static List<IFloor> CreateFloors(int count) =>
        Enumerable.Range(1, count)
            .Select(n => (IFloor)new Floor(n, new List<IPassenger>()))
            .ToList();

    [Fact]
    public void AddPassenger_ThrowsWhenAtCapacity()
    {
        var elevator = new PassengerElevator(2, CreateFloor(1));
        var p1 = new Mock<IPassenger>();
        var p2 = new Mock<IPassenger>();
        var p3 = new Mock<IPassenger>();

        elevator.Board(p1.Object);
        elevator.Board(p2.Object);

        Assert.Throws<ElevatorAtCapacityException>(() => elevator.Board(p3.Object));
    }

    [Fact]
    public void Controller_ThrowsFloorOutOfBoundsException_WhenTooManyFloors()
    {
        var floors = CreateFloors(ElevatorController.MaxFloorCount + 1);
        var elevators = new List<IElevator> { new PassengerElevator(10, floors[0]) };
        var strategy = new Mock<IDispatchStrategy>();

        Assert.Throws<FloorOutOfBoundsException>(() =>
            new ElevatorController(floors, elevators, strategy.Object));
    }

    [Fact]
    public void Controller_ThrowsTooManyElevatorsException_WhenElevatorCountExceedsMax()
    {
        var floors = CreateFloors(5);
        var elevators = Enumerable.Range(0, ElevatorController.MaxElevatorCount + 1)
            .Select(_ => (IElevator)new PassengerElevator(10, floors[0]))
            .ToList();
        var strategy = new Mock<IDispatchStrategy>();

        Assert.Throws<TooManyElevatorsException>(() =>
            new ElevatorController(floors, elevators, strategy.Object));
    }
}