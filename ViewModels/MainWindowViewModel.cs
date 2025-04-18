using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Heating_Optimization.Models;
using Heating_Optimization.Views;
using ReactiveUI;
namespace Heating_Optimization.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private UserControl _currentView;

    public UserControl CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
        
    }

    public void ShowUnits()
    {
        CurrentView = new UnitList();

    }

}
