namespace domain.interfaces;
public interface IElevator
{   
    public List<IPassenger> BoardedPassengers { get; set; }
    public int Capacity { get; set; }
    public IFloor CurrentFloor { get; set; }
    public void MoveUp();
    public void MoveDown();
}