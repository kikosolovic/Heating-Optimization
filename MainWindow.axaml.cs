using System;
using Avalonia.Controls;
using Heating_Optimization.Models;
using System.Globalization;

namespace Heating_Optimization;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        // SDM sdm = new SDM();
        // InitializeComponent();
        // Console.WriteLine(sdm.WinterPeriod[DateTime.Parse("1/3/2024 09:00:00")].ElectricityPrice);
        
        // Initialize AM and SDM (they auto-load their CSV data)
        AM am = new AM();
        SDM sdm = new SDM();

        // Create OPT instance
        OPT opt = new OPT(am, sdm);

        Console.WriteLine("Which period would you like to optimize?");
        Console.WriteLine("1- Calculate cheapest Production Costs for a period" );
        Console.WriteLine("2- Calculate the most sustainable combination for a period" );
        Console.WriteLine("3- Calculate a combination with an average of the first two options" );
        
        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine("Enter date (D/M/YYYY HH:mm): ");
    #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string userInput = Console.ReadLine();
    #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (DateTime.TryParseExact(userInput, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetTime))
            {
                opt.SortByProductionCost(targetTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        else if (choice == "2")
        {
            Console.WriteLine("Enter date (D/M/YYYY HH:mm): ");
    #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string userInput = Console.ReadLine();
    #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (DateTime.TryParseExact(userInput, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetTime))
            {
                opt.RankByCO2Emissions(targetTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        else if (choice == "3")
        {
            Console.WriteLine("Enter date (D/M/YYYY HH:mm): ");
    #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string userInput = Console.ReadLine();
    #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (DateTime.TryParseExact(userInput, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetTime))
            {
                opt.CalculateAverageRanking(targetTime);
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