using System;
using Avalonia.Controls;
using Heating_Optimization.Models;
using System.Globalization;

namespace Heating_Optimization;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        AM am = new AM();
        SDM sdm = new SDM();
        OPT opt = new OPT(am, sdm);
        
        InitializeComponent();

        Console.WriteLine("Which Scenario would you like to implemet?");
        Console.WriteLine("- Scenario 1");
        Console.WriteLine("- Scenario 2");

        string? choiceScenario = Console.ReadLine();
        
        // Convert the input to an integer (either 1 or 2)
        int scenario = 0;

        if (int.TryParse(choiceScenario, out scenario) && (scenario == 1 || scenario == 2))
        {
            Console.WriteLine($"You selected Scenario {scenario}");
            // Now you can use the 'scenario' variable to implement the chosen scenario
        }
        else
        {
            Console.WriteLine("Invalid choice. Please select 1 or 2.");
        }

        Console.WriteLine("What would you like to optimize?");
        Console.WriteLine("1- Calculate cheapest Production Costs for a period");
        Console.WriteLine("2- Calculate the most sustainable combination for a period");
        Console.WriteLine("3- Calculate a combination with an average of the first two options");

        string? choiceCalculation = Console.ReadLine();
        Action<DateTime>? selectedAction = choiceCalculation switch
        {
            "1" => (date) => opt.SortByProductionCost(date, scenario),
            "2" => (date) => opt.RankByCO2Emissions(date, scenario),    
            "3" => (date) => opt.CalculateAverageRanking(date, scenario), 
            _ => null
        };

        if (selectedAction != null)
        {
            Console.WriteLine("Enter date (D/M/YYYY HH:mm): ");
            string? userInput = Console.ReadLine();

            if (DateTime.TryParseExact(userInput, "d/M/yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetTime))
            {
                selectedAction(targetTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        else
        {
            Console.WriteLine("Invalid option. Please choose 1, 2, or 3.");
        }
    }
}  

// 01/03/2024 09:00   - ejemplo de fecha que sirve
