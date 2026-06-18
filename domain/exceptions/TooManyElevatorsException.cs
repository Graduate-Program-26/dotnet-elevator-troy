namespace domain.exceptions;

public class TooManyElevatorsException : Exception
{
    public TooManyElevatorsException(int numWantedElevators) : base(
        $"{numWantedElevators} is out of range, max elevators is 9")
    {
        NumWantedElevators = numWantedElevators;
    }

    public int NumWantedElevators { get; }
}