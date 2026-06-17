namespace domain.interfaces;

public interface IPassenger
{   
    public Guid Id { get; }
    public int WantFloor { get; set; }
    public int CurrentFloor { get; set; }
}