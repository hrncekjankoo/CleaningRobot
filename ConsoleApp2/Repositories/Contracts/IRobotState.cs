using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Repositories.Contracts;

public interface IRobotState
{
    void SetPosition(Position currentPosition);
    void SetBattery(int battery);
    Position GetPosition();
    int GetBattery();
}