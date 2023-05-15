using ConsoleApp2.Models;
using ConsoleApp2.Models.Cells;
using ConsoleApp2.Services.Contracts;

namespace ConsoleApp2.Services;

public class HistoryService : IHistoryService
{
    private readonly HashSet<Cell> _visited = new();
    private readonly HashSet<Cell> _cleaned = new();

    public void AddVisited(Position currentPosition)
    {
        var cell = new Cell {X = currentPosition.X, Y = currentPosition.Y};
        if (!_visited.Any(c => c.X == cell.X && c.Y == cell.Y))
        {
            _visited.Add(cell);
        }
    }

    public void AddCleaned(Position currentPosition)
    {
        var cell = new Cell {X = currentPosition.X, Y = currentPosition.Y};
        if (!_cleaned.Any(c => c.X == cell.X && c.Y == cell.Y))
        {
            _cleaned.Add(cell);
        }
    }

    public List<Cell> GetVisited()
    {
        return _visited.ToList();
    }
    
    public List<Cell> GetCleaned()
    {
        return _cleaned.ToList();
    }
}