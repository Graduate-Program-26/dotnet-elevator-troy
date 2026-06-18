using domain.exceptions;

namespace domain.tests.implementations;

using domain.enums;
using domain.implementations;
using domain.interfaces;
using Moq;

public class PassengerElevatorTests
{
    private static Mock<IFloor> CreateFloor(int floorNumber)
    {
        var mock = new Mock<IFloor>();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock;
    }

    [Fact]
    public void Constructor_InitializesWithStartingFloor()
    {
        var floor = CreateFloor(1);
        var elevator = new PassengerElevator(10, floor.Object);

        Assert.Equal(floor.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveUp_WhenNextFloorExists_UpdatesCurrentFloor()
    {
        var floor1 = CreateFloor(1);
        var floor2 = CreateFloor(2);
        var elevator = new PassengerElevator(10, floor1.Object);

        elevator.MoveUp(floor2.Object);

        Assert.Equal(floor2.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveUp_CalledMultipleTimes_AdvancesFloorCorrectly()
    {
        var floor1 = CreateFloor(1);
        var floor2 = CreateFloor(2);
        var floor3 = CreateFloor(3);
        var elevator = new PassengerElevator(10, floor1.Object);

        elevator.MoveUp(floor2.Object);
        elevator.MoveUp(floor3.Object);

        Assert.Equal(floor3.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveDown_WhenPreviousFloorExists_UpdatesCurrentFloor()
    {
        var floor1 = CreateFloor(1);
        var floor2 = CreateFloor(2);
        var elevator = new PassengerElevator(10, floor2.Object);

        elevator.MoveDown(floor1.Object);

        Assert.Equal(floor1.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveDown_CalledMultipleTimes_DescendsFloorCorrectly()
    {
        var floor1 = CreateFloor(1);
        var floor2 = CreateFloor(2);
        var floor3 = CreateFloor(3);
        var elevator = new PassengerElevator(10, floor3.Object);

        elevator.MoveDown(floor2.Object);
        elevator.MoveDown(floor1.Object);

        Assert.Equal(floor1.Object, elevator.CurrentFloor);
    }
}