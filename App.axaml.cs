using System;
using System.Data;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Heating_Optimization.Models;

namespace Heating_Optimization;

public partial class App : Application
{
    public override void Initialize()
    {
        // SDM sdm = new SDM();
        // Console.WriteLine(sdm.SummerPeriod[DateTime.Parse("8/11/2024 0:00")].ElectricityPrice);
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}