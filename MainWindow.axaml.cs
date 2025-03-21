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

        Console.WriteLine("Enter date (DD/MM/YYYY HH:mm): ");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string userInput = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (DateTime.TryParseExact(userInput, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetTime))
        {
            opt.CalculateElectricityCosts(targetTime);
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }
}

// 01/03/2024 09:00