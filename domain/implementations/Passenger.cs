using domain.interfaces;

namespace domain.implementations;

public class Passenger : IPassenger
{
    public Guid Id { get; } = Guid.NewGuid();
    public int WantFloor { get; set; }
    public int CurrentFloor { get; set; }

    public Passenger(int wantFloor, int currentFloor)
    {
        WantFloor = wantFloor;
        CurrentFloor = currentFloor;
    }
}