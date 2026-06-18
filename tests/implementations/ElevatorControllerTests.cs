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
        Mock<IFloor> mock = new();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock.Object;
    }

    private static List<IFloor> CreateFloors(int count)
    {
        return Enumerable.Range(1, count)
            .Select(n => (IFloor)new Floor(n, new List<IPassenger>()))
            .ToList();
    }

    [Fact]
    public void AddPassenger_ThrowsWhenAtCapacity()
    {
        PassengerElevator elevator = new(2, CreateFloor(1));
        Mock<IPassenger> p1 = new();
        Mock<IPassenger> p2 = new();
        Mock<IPassenger> p3 = new();

        elevator.Board(p1.Object);
        elevator.Board(p2.Object);

        Assert.Throws<ElevatorAtCapacityException>(() => elevator.Board(p3.Object));
    }

    [Fact]
    public void Controller_ThrowsFloorOutOfBoundsException_WhenTooManyFloors()
    {
        List<IFloor> floors = CreateFloors(ElevatorController.MaxFloorCount + 1);
        List<IElevator> elevators = new() { new PassengerElevator(10, floors[0]) };
        Mock<IDispatchStrategy> strategy = new();

        Assert.Throws<FloorOutOfBoundsException>(() =>
            new ElevatorController(floors, elevators, strategy.Object));
    }

    [Fact]
    public void Controller_ThrowsTooManyElevatorsException_WhenElevatorCountExceedsMax()
    {
        List<IFloor> floors = CreateFloors(5);
        List<IElevator> elevators = Enumerable.Range(0, ElevatorController.MaxElevatorCount + 1)
            .Select(_ => (IElevator)new PassengerElevator(10, floors[0]))
            .ToList();
        Mock<IDispatchStrategy> strategy = new();

        Assert.Throws<TooManyElevatorsException>(() =>
            new ElevatorController(floors, elevators, strategy.Object));
    }
}