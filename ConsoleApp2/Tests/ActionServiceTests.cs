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
    
    [Fact]
    public void MoveBackSuccessfully()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 0, Facing = Direction.E });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: true);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.True(result);
        Assert.Equal(97, _robotState.GetBattery());
        Assert.Equal(Direction.E, position.Facing);
        Assert.Equal(2, position.X);
        Assert.Equal(0, position.Y);
    }
    
    [Fact]
    public void MoveAdvanceSuccessfully()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 0, Facing = Direction.W });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: false);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.True(result);
        Assert.Equal(98, _robotState.GetBattery());
        Assert.Equal(Direction.W, position.Facing);
        Assert.Equal(2, position.X);
        Assert.Equal(0, position.Y);
    }
    
    [Fact]
    public void CantMoveBackOutsideOfMap()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 0, Facing = Direction.W });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: true);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.False(result);
        Assert.Equal(97, _robotState.GetBattery());
        Assert.Equal(Direction.W, position.Facing);
        Assert.Equal(3, position.X);
        Assert.Equal(0, position.Y);
    }
    
    [Fact]
    public void CantMoveAdvanceOutsideOfMap()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 0, Facing = Direction.E });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: false);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.False(result);
        Assert.Equal(98, _robotState.GetBattery());
        Assert.Equal(Direction.E, position.Facing);
        Assert.Equal(3, position.X);
        Assert.Equal(0, position.Y);
    }
    
    [Fact]
    public void CantMoveBackToObstacle()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 1, Facing = Direction.W });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: false);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.False(result);
        Assert.Equal(98, _robotState.GetBattery());
        Assert.Equal(Direction.W, position.Facing);
        Assert.Equal(3, position.X);
        Assert.Equal(1, position.Y);
    }
    
    [Fact]
    public void CantMoveAdvanceToObstacle()
    {
        // Given
        InitializeServices();
        _robotState.SetBattery(100);
        _robotState.SetPosition(new Position { X = 3, Y = 1, Facing = Direction.E });
        
        // When
        var result = _actionService.TryMove(GetMap(), isBack: true);
        
        // Then
        var position = _robotState.GetPosition();
        Assert.False(result);
        Assert.Equal(97, _robotState.GetBattery());
        Assert.Equal(Direction.E, position.Facing);
        Assert.Equal(3, position.X);
        Assert.Equal(1, position.Y);
    }

    private void InitializeServices()
    {
        _historyService = new HistoryService();
        _robotState = new RobotState();
        _actionService = new ActionService(_historyService, _robotState);
    }

    private static CellStatus?[,] GetMap()
    {
        return new CellStatus?[,]
        {
            { CellStatus.S, CellStatus.S, CellStatus.S, CellStatus.S },
            { CellStatus.S, CellStatus.S, CellStatus.C, CellStatus.S },
            { CellStatus.S, CellStatus.S, CellStatus.S, CellStatus.S },
            { CellStatus.S, null, CellStatus.S, CellStatus.S }
        };
    }
}