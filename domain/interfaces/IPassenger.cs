namespace domain.interfaces;

public interface IPassenger
{
    Guid Id { get; }
    int WantFloor { get; set; }
    int CurrentFloor { get; set; }
}