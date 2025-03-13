using System;
using Avalonia.Controls;
using Heating_Optimization.Models;
namespace Heating_Optimization;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        SDM sdm = new SDM();
        InitializeComponent();
        Console.WriteLine(sdm.SummerPeriod[DateTime.Parse("8/11/2024 0:00")].ElectricityPrice);

    }
}