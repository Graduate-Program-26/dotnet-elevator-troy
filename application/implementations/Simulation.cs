using domain.enums;
using domain.interfaces;
using domain.implementations;

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

    private int _spawned;
    private int _delivered;

    public bool IsComplete => _spawned >= _passengerLimit && _delivered >= _passengerLimit;
    public int Delivered => _delivered;
    
    public Simulation(
        IList<IFloor> floors,
        IList<IElevator> elevators,
        IElevatorController controller,
        int passengerLimit,
        Random random)
    {
        _floors = floors;
        _elevators = elevators;
        _controller = controller;
        _passengerLimit = passengerLimit;
        _random = random;
        _floorsByNumber = floors.ToDictionary(f => f.FloorNumber);
        //dict is good way to store elevators as theyre now accessible by dictionary index
        //but it has quirk where now each elevator spawns on its index floor instead of first floor
        _targets = elevators.ToDictionary(e => e, e => (IFloor?)null);
    }

    public void Tick()
    {
        TrySpawnPassenger();
        AssignTargets();
        StepElevators();
    }

    private void TrySpawnPassenger()
    {
        if (_spawned >= _passengerLimit || _random.NextDouble() >= SpawnChance) return;

        var origin = PickRandomFloor();
        var destination = PickDifferentFloor(origin);

        origin.WaitingPassengers.Add(new Passenger(destination.FloorNumber, origin.FloorNumber));
        _spawned++;
    }

    private IFloor PickRandomFloor() =>
        _floors[_random.Next(_floors.Count)];

    private IFloor PickDifferentFloor(IFloor excluded)
    {
        var candidates = _floors.Where(f => f.FloorNumber != excluded.FloorNumber).ToList();
        return candidates[_random.Next(candidates.Count)];
    }

    private void AssignTargets()
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

            var elevator = _controller.SelectElevator(floor, idleElevators);
            _targets[elevator] = floor;
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

        var diff = target.FloorNumber - elevator.CurrentFloor.FloorNumber;

        if (diff > 0)
        {
            elevator.SetDirection(Direction.Upwards);
            elevator.MoveUp(FloorByNumber(elevator.CurrentFloor.FloorNumber + 1));
        }
        else if (diff < 0)
        {
            elevator.SetDirection(Direction.Downwards);
            elevator.MoveDown(FloorByNumber(elevator.CurrentFloor.FloorNumber - 1));
        }
        else
        {
            HandleArrival(elevator, target);
        }
    }

    private void HandleArrival(IElevator elevator, IFloor floor)
    {
        elevator.SetDirection(Direction.None);
        BoardWaitingPassengers(elevator, floor);
        DeboardArrivedPassengers(elevator);
        _targets[elevator] = NextTarget(elevator);
    }

    private void BoardWaitingPassengers(IElevator elevator, IFloor floor)
    {
        foreach (var passenger in floor.WaitingPassengers.ToList())
        {
            if (elevator.BoardedPassengers.Count >= elevator.Capacity) break;
            elevator.Board(passenger);
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
        }
    }

    private IFloor? NextTarget(IElevator elevator) =>
        elevator.BoardedPassengers.Count > 0
            ? FloorByNumber(elevator.BoardedPassengers.First().WantFloor)
            : null;

    private IFloor FloorByNumber(int number) => _floorsByNumber[number];
}