using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Models.Contracts;

public class Input
{
    public CellStatus?[,] Map { get; set; } = null!;
    public Position Start { get; set; } = null!;
    public Command[] Commands { get; set; } = null!;
    public int Battery { get; set; } 
}