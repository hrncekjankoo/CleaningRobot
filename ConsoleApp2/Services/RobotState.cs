using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;
using ConsoleApp2.Services.Contracts;

namespace ConsoleApp2.Services;

public class RobotState : IRobotState
{
    private Position _position = null!;
    private int _battery;

    public void SetPosition(Position currentPosition)
    {
        _position = currentPosition;
    }

    public void SetBattery(int battery)
    {
        _battery = battery;
    }

    public Position GetPosition()
    {
        return _position;
    }
    
    public int GetBattery()
    {
        return _battery;
    }
}