using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;
using ConsoleApp2.Models.Contracts;
using ConsoleApp2.Repositories.Contracts;
using ConsoleApp2.Services.Contracts;

namespace ConsoleApp2.Services;

public class RobotService : IRobotService
{
    private readonly IActionService _actionService;
    private readonly IHistoryRepository _historyService;
    private readonly IRobotState _robotState;

    public RobotService(IActionService actionService, IHistoryRepository historyService, IRobotState robotState)
    {
        _actionService = actionService;
        _historyService = historyService;
        _robotState = robotState;
    }
    
    public Output Start(Input input)
    {
        _robotState.SetPosition(input.Start);
        _historyService.AddVisited(input.Start);
        _robotState.SetBattery(input.Battery);
        DoCommands(input.Map, input.Commands);

        return new Output
        {
            Visited = _historyService.GetVisited(),
            Cleaned = _historyService.GetCleaned(),
            Final = _robotState.GetPosition(),
            Battery = _robotState.GetBattery()
        };
    }

    private bool DoCommands(CellStatus?[,] map, IEnumerable<Command> commands, bool isBackup = false)
    {
        foreach (var command in commands)
        {
            var done = command switch
            {
                Command.A => TryAdvance(map, isBackup),
                Command.B => TryBack(map, isBackup),
                Command.TL => _actionService.TryTurnLeft(),
                Command.TR => _actionService.TryTurnRight(),
                Command.C => _actionService.TryClean(),
            };

            if (!done) return false;
        }

        return true;
    }

    private bool TryAdvance(CellStatus?[,] map, bool isBackup)
    {
        var moved = _actionService.TryMove(map, isBack: false);
        if (moved) return true;

        if (!isBackup) return DoBackupMoves(map);

        return false;
    }
    
    private bool TryBack(CellStatus?[,] map, bool isBackup)
    {
        var moved = _actionService.TryMove(map, isBack: true);
        if (moved) return true;
        
        if (!isBackup) return DoBackupMoves(map);

        return false;
    }
    
    private bool DoBackupMoves(CellStatus?[,] map)
    {
        foreach (var commandSet in GetBackupCommands())
        {
            var done = DoCommands(map, commandSet, isBackup: true);
            if (done) return true;
        }

        return false;
    }
    
    private static List<List<Command>> GetBackupCommands()
    {
        return new List<List<Command>>
        {
            new() { Command.TR, Command.A },
            new() { Command.TL, Command.B, Command.TR, Command.A },
            new() { Command.TL, Command.TL, Command.A },
            new() { Command.TR, Command.B, Command.TR, Command.A },
            new() { Command.TL, Command.TL, Command.A }
        };
    }
}