using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Heating_Optimization.Models;
using ReactiveUI;

namespace Heating_Optimization.ViewModels;

public class UnitListViewModel : ReactiveObject
{
    public ObservableCollection<PU> PUs { get; set; }
    public ReactiveCommand<Unit, Unit> ToggleCommand { get; }



    public UnitListViewModel()
    {
        AM am = new AM();

        PUs = new ObservableCollection<PU>(am.ProductionUnits);
        ToggleCommand = ReactiveCommand.Create(toggleChange);

    }



    public void toggleChange()
    {
        Console.WriteLine("toggle");
    }



}
