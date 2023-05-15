using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Services.Contracts;

public interface IRobotState
{
    void SetPosition(Position currentPosition);
    void SetBattery(int battery);
    Position GetPosition();
    int GetBattery();
}