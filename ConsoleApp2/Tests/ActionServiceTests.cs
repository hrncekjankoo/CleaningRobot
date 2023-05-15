using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;
using ConsoleApp2.Services;
using Xunit;

namespace ConsoleApp2.Tests;

public class ActionServiceTests
{
    private ActionService _actionService = null!;
    private RobotState _robotState = null!;
    private HistoryService _historyService = null!;
    
    [Fact]
    public void TurnLeftWithSuccessfully()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 2, Y = 2, Facing = Direction.S });
        
        // When
        var result = _actionService.TryTurnLeft();
        
        // Then
        Assert.True(result);
        Assert.Equal(99, _robotState.GetBattery());
        Assert.Equal(Direction.E, _robotState.GetPosition().Facing);
    }
    
    [Fact]
    public void TurnLeftWithWillNotWorkWithoutEnoughBattery()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(0);
        _robotState.SetPosition(new Position { X = 2, Y = 2, Facing = Direction.S });
        
        // When
        var result = _actionService.TryTurnLeft();
        
        // Then
        Assert.False(result);
    }

    private void InitializeServices()
    {
        _historyService = new HistoryService();
        _robotState = new RobotState();
        _actionService = new ActionService(_historyService, _robotState);
    }
}