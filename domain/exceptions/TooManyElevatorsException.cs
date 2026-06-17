namespace domain.exceptions;

public class TooManyElevatorsException : Exception
{
    public int NumWantedElevators {get; }

    public TooManyElevatorsException(int numWantedElevators) : base($"{numWantedElevators} is out of range, max elevators is 9")
    {
        NumWantedElevators = numWantedElevators;
    }
}