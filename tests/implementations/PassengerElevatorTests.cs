using domain.implementations;
using domain.interfaces;

using Moq;

namespace domain.tests.implementations;

public class PassengerElevatorTests
{
    private static Mock<IFloor> CreateFloor(int floorNumber)
    {
        Mock<IFloor> mock = new();
        mock.Setup(f => f.FloorNumber).Returns(floorNumber);
        return mock;
    }

    [Fact]
    public void Constructor_InitializesWithStartingFloor()
    {
        Mock<IFloor> floor = CreateFloor(1);
        PassengerElevator elevator = new(10, floor.Object);

        Assert.Equal(floor.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveUp_WhenNextFloorExists_UpdatesCurrentFloor()
    {
        Mock<IFloor> floor1 = CreateFloor(1);
        Mock<IFloor> floor2 = CreateFloor(2);
        PassengerElevator elevator = new(10, floor1.Object);

        elevator.MoveUp(floor2.Object);

        Assert.Equal(floor2.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveUp_CalledMultipleTimes_AdvancesFloorCorrectly()
    {
        Mock<IFloor> floor1 = CreateFloor(1);
        Mock<IFloor> floor2 = CreateFloor(2);
        Mock<IFloor> floor3 = CreateFloor(3);
        PassengerElevator elevator = new(10, floor1.Object);

        elevator.MoveUp(floor2.Object);
        elevator.MoveUp(floor3.Object);

        Assert.Equal(floor3.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveDown_WhenPreviousFloorExists_UpdatesCurrentFloor()
    {
        Mock<IFloor> floor1 = CreateFloor(1);
        Mock<IFloor> floor2 = CreateFloor(2);
        PassengerElevator elevator = new(10, floor2.Object);

        elevator.MoveDown(floor1.Object);

        Assert.Equal(floor1.Object, elevator.CurrentFloor);
    }

    [Fact]
    public void MoveDown_CalledMultipleTimes_DescendsFloorCorrectly()
    {
        Mock<IFloor> floor1 = CreateFloor(1);
        Mock<IFloor> floor2 = CreateFloor(2);
        Mock<IFloor> floor3 = CreateFloor(3);
        PassengerElevator elevator = new(10, floor3.Object);

        elevator.MoveDown(floor2.Object);
        elevator.MoveDown(floor1.Object);

        Assert.Equal(floor1.Object, elevator.CurrentFloor);
    }
}