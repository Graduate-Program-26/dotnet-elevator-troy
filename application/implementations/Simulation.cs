using domain.enums;
using domain.interfaces;
using domain.implementations;

using Serilog;

namespace application.implementations;

public sealed class Simulation
{
    private const double SpawnChance = 0.4;
    public const int MinPassengerCount = 1;
    public const int MaxPassengerCount = 100;

    private readonly IList<IFloor> _floors;
    private readonly IList<IElevator> _elevators;
    private readonly IElevatorController _controller;
    private readonly Dictionary<int, IFloor> _floorsByNumber;
    private readonly Dictionary<IElevator, IFloor?> _targets;
    private readonly Random _random;
    private readonly int _passengerLimit;
    private readonly ILogger _logger;

    private int _spawned;
    private int _delivered;

    public bool IsComplete => _spawned >= _passengerLimit && _delivered >= _passengerLimit;
    public int Delivered => _delivered;

    public Simulation(
        IList<IFloor> floors,
        IList<IElevator> elevators,
        IElevatorController controller,
        int passengerLimit,
        Random random,
        ILogger logger)
    {
        _floors = floors;
        _elevators = elevators;
        _controller = controller;
        _passengerLimit = passengerLimit;
        _random = random;
        _floorsByNumber = floors.ToDictionary(f => f.FloorNumber);
        _targets = elevators.ToDictionary(e => e, e => (IFloor?)null);
        _logger = logger;
    }

    public void Tick()
    {
        TrySpawnPassenger();
        AssignTargetFloors();
        StepElevators();
    }

    private void TrySpawnPassenger()
    {
        if (_spawned >= _passengerLimit || _random.NextDouble() >= SpawnChance) return;

        var origin = PickRandomFloor();
        var destination = PickDifferentFloor(origin);

        origin.WaitingPassengers.Add(new Passenger(destination.FloorNumber, origin.FloorNumber));
        _spawned++;
        _logger.Information("Passenger spawned at {OriginFloor} wants to go to {DestinationFloor}", origin.FloorNumber, destination.FloorNumber);
    }

    private IFloor PickRandomFloor() =>
        _floors[_random.Next(_floors.Count)];

    private IFloor PickDifferentFloor(IFloor excluded)
    {
        var candidates = _floors.Where(f => f.FloorNumber != excluded.FloorNumber).ToList();
        return candidates[_random.Next(candidates.Count)];
    }

    private void AssignTargetFloors()
    {
        var floorsNeedingService = _floors
            .Where(f => f.WaitingPassengers.Count > 0 && !_targets.ContainsValue(f))
            .ToList();

        var idleElevators = _elevators
            .Where(e => _targets[e] == null)
            .ToList();

        if (idleElevators.Count == 0) return;

        foreach (var floor in floorsNeedingService)
        {
            if (idleElevators.Count == 0) break;

            var elevator = _controller.Dispatch(floor, idleElevators);
            _targets[elevator] = floor;
            var elevatorNumber = _elevators.IndexOf(elevator) + 1;
            _logger.Information("Elevator {ElevatorNumber} dispatched to floor {FloorNumber}", elevatorNumber, floor.FloorNumber);
            idleElevators.Remove(elevator);
        }
    }

    private void StepElevators()
    {
        foreach (var elevator in _elevators)
            StepElevator(elevator);
    }

    private void StepElevator(IElevator elevator)
    {
        var target = _targets[elevator];
        if (target == null) return;

        if (elevator.CurrentFloor.FloorNumber == target.FloorNumber)
            HandleArrival(elevator, target);
        else
            _controller.MoveToFloor(elevator, target);
    }

    private void HandleArrival(IElevator elevator, IFloor floor)
    {
        elevator.SetDirection(Direction.None);
        BoardWaitingPassengers(elevator, floor);
        DeboardArrivedPassengers(elevator);
        _targets[elevator] = NextTargetFloor(elevator);
    }

    private void BoardWaitingPassengers(IElevator elevator, IFloor floor)
    {
        foreach (var passenger in floor.WaitingPassengers.ToList())
        {
            int elevatorNumber = _elevators.IndexOf(elevator) + 1;
            if (elevator.BoardedPassengers.Count >= elevator.Capacity)
            {
                _logger.Warning("Elevator {ElevatorNumber} reached capacity", elevatorNumber);
                break;
            }
            elevator.Board(passenger);
            _logger.Information("Passenger boarded elevator {ElevatorNumber} at capacity: {Count}/{Capacity} passengers", elevatorNumber, elevator.BoardedPassengers.Count, elevator.Capacity);
            floor.WaitingPassengers.Remove(passenger);
        }
    }

    private void DeboardArrivedPassengers(IElevator elevator)
    {
        foreach (var passenger in elevator.BoardedPassengers.ToList())
        {
            if (passenger.WantFloor != elevator.CurrentFloor.FloorNumber) continue;
            elevator.Deboard(passenger);
            _delivered++;
            int elevatorNumber = _elevators.IndexOf(elevator) + 1;
            _logger.Information("Elevator {ElevatorNumber} delivered passenger to floor {FloorNumber}", elevatorNumber, elevator.CurrentFloor.FloorNumber);
        }
    }

    private IFloor? NextTargetFloor(IElevator elevator) =>
        elevator.BoardedPassengers.Count > 0
            ? _controller.GetFloor(elevator.BoardedPassengers.First().WantFloor)
            : null;
}