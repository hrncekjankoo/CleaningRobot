using ConsoleApp2.Models.Contracts;
using ConsoleApp2.Repositories;
using ConsoleApp2.Repositories.Contracts;
using ConsoleApp2.Services;
using ConsoleApp2.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleApp2;

internal static class Program
{
    private static void Main(string[]? args)
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IRobotService, RobotService>()
            .AddSingleton<IHistoryRepository, HistoryRepository>()
            .AddScoped<IActionService, ActionService>()
            .AddSingleton<IRobotState, RobotState>()
            .BuildServiceProvider();

        if (args is {Length: < 2})
        {
            throw new ArgumentException("Please provide input and output filenames.");
        }
        
        var inputFilename = args[0];
        var outputFilename = args[1];
        
        var text = File.ReadAllText(inputFilename);
        text = text.Replace("\"null\"", "null");
        
        var parsedInput = JsonConvert.DeserializeObject<Input>(text);
        var demoService = serviceProvider.GetRequiredService<IRobotService>();

        if (parsedInput == null)
        {
            throw new ArgumentException("Input file invalid.");
        }
        
        var output = demoService.Start(parsedInput);
        var outputText = JsonConvert.SerializeObject(output, new StringEnumConverter());
        
        File.WriteAllText(outputFilename, outputText);
    }
}