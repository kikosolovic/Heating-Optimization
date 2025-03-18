using System;
using System.Data;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Heating_Optimization.Models;
using Heating_Optimization.Views;
using Heating_Optimization.ViewModels;

namespace Heating_Optimization;

public partial class App : Application
{
    public override void Initialize()
    {
        SDM sdm = new SDM();
        AM am = new AM();
        Console.WriteLine("electricity production for PU nm 2");
        Console.WriteLine(am.ProductionUnits[3].ElectricityProduction);
        Console.WriteLine("electricity price for 8/11/2024 0:00");
        Console.WriteLine(sdm.SummerPeriod[DateTime.Parse("8/11/2024 0:00")].ElectricityPrice);



        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // desktop.MainWindow = new MainWindow();
            desktop.MainWindow = new Login
            {
                DataContext = new LoginViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}