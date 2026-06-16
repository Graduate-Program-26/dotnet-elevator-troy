using domain.enums;

namespace domain.interfaces;
public interface IElevator
{
    List<IPassenger> BoardedPassengers { get; set; }
    int Capacity { get; set; }
    IFloor CurrentFloor { get; set; }
    Direction WishDirection { get; set; }

    void MoveUp(IFloor nextFloor);
    void MoveDown(IFloor nextFloor);
    void Board(IPassenger passenger);
    void Deboard(IPassenger passenger);
    void SetDirection(Direction direction);
}