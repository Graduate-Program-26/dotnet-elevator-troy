using domain.interfaces;

namespace domain.implementations;

public class Passenger : IPassenger
{
    public Guid Id { get; } = Guid.NewGuid();
    public string PassengerName { get; set; }
    public int WantFloor { get; set; }
    public int CurrentFloor { get; set; }

    public Passenger(string passengerName, int wantFloor, int currentFloor)
    {
        PassengerName = passengerName;
        WantFloor = wantFloor;
        CurrentFloor = currentFloor;
    }
}