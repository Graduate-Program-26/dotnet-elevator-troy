namespace domain.exceptions;

public class ElevatorAtCapacityException : Exception
{
    public int Capacity { get; }

    public ElevatorAtCapacityException(int capacity)
        : base($"Elevator has a capacity of {capacity} passengers and can't go over that.")
    {
        Capacity = capacity;
    }
}