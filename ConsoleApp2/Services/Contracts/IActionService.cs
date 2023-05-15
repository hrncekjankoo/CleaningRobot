using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Services.Contracts;

public interface IActionService
{
    bool TryMove(CellStatus?[,] map, bool isBack);
    bool TryTurnLeft();
    bool TryTurnRight();
    bool TryClean();
}