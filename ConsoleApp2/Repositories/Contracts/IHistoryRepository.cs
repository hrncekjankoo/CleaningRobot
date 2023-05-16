using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Repositories.Contracts;

public interface IHistoryRepository
{
    void AddVisited(Position currentPosition);
    void AddCleaned(Position currentPosition);
    List<Cell> GetVisited();
    List<Cell> GetCleaned();
}