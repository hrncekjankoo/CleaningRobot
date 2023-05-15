using ConsoleApp2.Models.Cells;

namespace ConsoleApp2.Models.Contracts;

public class Output
{
    public List<Cell> Visited { get; set; } = new(); 
    public List<Cell> Cleaned { get; set; } = new(); 
    public Position Final { get; set; } = null!;
    public int Battery { get; set; } 
}