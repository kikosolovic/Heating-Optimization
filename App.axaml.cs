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

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        DataLoader.LoadData();
        Console.WriteLine(SDM.SummerPeriod.Count);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {


            // desktop.MainWindow = new Login
            // {
            //     DataContext = new LoginViewModel(),
            // };
            var optimizer = new OPT();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(optimizer)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}