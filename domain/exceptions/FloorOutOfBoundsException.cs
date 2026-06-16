namespace domain.exceptions;

public class FloorOutOfBoundsException : Exception
{
    public int AttemptedFloor {get; }

    public FloorOutOfBoundsException(int attemptedFloor) : base($"{attemptedFloor} does not exist or is out of range for the current set of floors.")
    {
        AttemptedFloor = attemptedFloor;
    }
}