namespace domain.interfaces;
public interface IElevator
{
    public int Capacity { get; set; }
    public IFloor CurrentFloor { get; set; }
    public void MoveUp();
    public void MoveDown();
}