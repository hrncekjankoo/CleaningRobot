using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;
using ConsoleApp2.Repositories.Contracts;
using ConsoleApp2.Services.Contracts;

namespace ConsoleApp2.Services;

public class ActionService : IActionService
{
    private readonly IHistoryRepository _historyRepository;
    private readonly IRobotState _robotState;

    private const int AdvanceConsumption = 2;
    private const int BackConsumption = 3;
    private const int CleanConsumption = 5;
    private const int TurnConsumption = 1;

    public ActionService(IHistoryRepository historyRepository, IRobotState robotState)
    {
        _historyRepository = historyRepository;
        _robotState = robotState;
    }
    
    public bool TryTurnLeft()
    {
        var charged = ConsumeBattery(TurnConsumption);
        if (!charged) return false;

        var currentPosition = _robotState.GetPosition();
        currentPosition.Facing = currentPosition switch
        {
            {Facing: Direction.N} => Direction.W,
            {Facing: Direction.E} => Direction.N,
            {Facing: Direction.W} => Direction.S,
            {Facing: Direction.S} => Direction.E
        };
        
        _robotState.SetPosition(currentPosition);

        return true;
    }

    public bool TryTurnRight()
    {
        var charged = ConsumeBattery(TurnConsumption);
        if (!charged) return false;
        
        var currentPosition = _robotState.GetPosition();
        currentPosition.Facing = currentPosition switch
        {
            {Facing: Direction.N} => Direction.E,
            {Facing: Direction.E} => Direction.S,
            {Facing: Direction.W} => Direction.N,
            {Facing: Direction.S} => Direction.W
        };
        
        _robotState.SetPosition(currentPosition);
        
        return true;
    }

    public bool TryClean()
    {
        var charged = ConsumeBattery(CleanConsumption);
        
        var currentPosition = _robotState.GetPosition();
        _historyRepository.AddCleaned(currentPosition);
        return charged;
    }

    public bool TryMove(CellStatus?[,] map, bool isBack)
    {
        var width = map.GetLength(0);
        var height = map.GetLength(1);

        var currentPosition = _robotState.GetPosition();
        if (isBack)
        {
            var drainedBatteryForBackAction = ConsumeBattery(BackConsumption);
            if (!drainedBatteryForBackAction) return false;
            
            var canMoveBack = currentPosition switch
            {
                { Facing: Direction.N } => currentPosition.Y != height - 1 &&
                                         map[currentPosition.Y + 1, currentPosition.X] != null &&
                                         map[currentPosition.Y + 1, currentPosition.X] != CellStatus.C,
                { Facing: Direction.E } => currentPosition.X != 0 &&
                                         map[currentPosition.Y, currentPosition.X - 1] != null &&
                                         map[currentPosition.Y, currentPosition.X - 1] != CellStatus.C,
                { Facing: Direction.W } => currentPosition.X != width - 1 &&
                                         map[currentPosition.Y, currentPosition.X + 1] != null &&
                                         map[currentPosition.Y, currentPosition.X + 1] != CellStatus.C,
                { Facing: Direction.S } => currentPosition.Y != 0 &&
                                         map[currentPosition.Y - 1, currentPosition.X] != null &&
                                         map[currentPosition.Y - 1, currentPosition.X] != CellStatus.C
            };
            
            if (canMoveBack) Back();
            return canMoveBack;
        }

        var drainedBatteryForAdvanceAction = ConsumeBattery(AdvanceConsumption);
        if (!drainedBatteryForAdvanceAction) return false;
        
        var canMoveAdvance =  currentPosition switch
            {
                { Facing: Direction.N } => currentPosition.Y != 0 &&
                                         map[currentPosition.Y - 1, currentPosition.X] != null &&
                                         map[currentPosition.Y - 1, currentPosition.X] != CellStatus.C,
                { Facing: Direction.E } => currentPosition.X != width - 1 &&
                                         map[currentPosition.Y, currentPosition.X + 1] != null &&
                                         map[currentPosition.Y, currentPosition.X + 1] != CellStatus.C,
                { Facing: Direction.W } => currentPosition.X != 0 &&
                                         map[currentPosition.Y, currentPosition.X - 1] != null &&
                                         map[currentPosition.Y, currentPosition.X - 1] != CellStatus.C,
                { Facing: Direction.S } => currentPosition.Y == height - 1 && 
                                         map[currentPosition.Y + 1, currentPosition.X] != null &&
                                         map[currentPosition.Y + 1 ,currentPosition.X] != CellStatus.C 
            };

        if (canMoveAdvance) Advance();
        return canMoveAdvance;
    }
    
    private void Advance()
    {
        var currentPosition = _robotState.GetPosition();
        switch (currentPosition.Facing)
        {
            case Direction.N:
                currentPosition.Y -= 1;
                break;
            case Direction.W:
                currentPosition.X -= 1;
                break;
            case Direction.S:
                currentPosition.Y += 1;
                break;
            case Direction.E:
                currentPosition.X += 1;
                break;
        }
        
        _historyRepository.AddVisited(currentPosition);
        _robotState.SetPosition(currentPosition);
    }
    
    private void Back()
    {
        var currentPosition = _robotState.GetPosition();
        switch (currentPosition.Facing)
        {
            case Direction.N: 
                currentPosition.Y += 1;
                break;
            case Direction.W:
                currentPosition.X += 1;
                break;
            case Direction.S:
                currentPosition.Y -= 1;
                break;
            case Direction.E:
                currentPosition.X -= 1;
                break;
        }
        
        _historyRepository.AddVisited(currentPosition);
        _robotState.SetPosition(currentPosition);
    }
    
    private bool ConsumeBattery(int consumption)
    {
        var battery = _robotState.GetBattery();
        
        if (battery <= consumption) return false;
        battery -= consumption;

        _robotState.SetBattery(battery);
        return true;
    }
}