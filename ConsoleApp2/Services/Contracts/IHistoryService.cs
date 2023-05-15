using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Services.Contracts;

public interface IHistoryService
{
    void AddVisited(Position currentPosition);
    void AddCleaned(Position currentPosition);
    List<Cell> GetVisited();
    List<Cell> GetCleaned();
}