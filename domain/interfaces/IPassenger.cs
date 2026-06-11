namespace domain.interfaces;

public interface IPassenger
{   
    public String PassengerName { get; set; }
    public int WantFloor { get; set; }
    public int CurrentFloor { get; set; }
}