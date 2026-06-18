namespace domain.exceptions;

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException(int currentFloor, int boundaryFloor)
        : base($"Cant set direction from floor {currentFloor} the boundary floor is {boundaryFloor}")
    {
        CurrentFloor = currentFloor;
        BoundaryFloor = boundaryFloor;
    }

    public int CurrentFloor { get; }
    public int BoundaryFloor { get; }
}