using domain.enums;

namespace domain.interfaces;

public interface IElevator
{
    IReadOnlyList<IPassenger> BoardedPassengers { get; }
    int Capacity { get; }
    IFloor CurrentFloor { get; }
    Direction WishDirection { get; }

    void MoveUp(IFloor nextFloor);
    void MoveDown(IFloor nextFloor);
    void Board(IPassenger passenger);
    void Deboard(IPassenger passenger);
    void SetDirection(Direction direction);
}