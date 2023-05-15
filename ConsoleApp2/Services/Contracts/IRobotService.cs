using ConsoleApp2.Models;
using ConsoleApp2.Models.Contracts;

namespace ConsoleApp2.Services.Contracts;

public interface IRobotService
{
    Output Start(Input input);
}