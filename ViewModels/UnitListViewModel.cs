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

        PUs = new ObservableCollection<PU>(AM.ProductionUnits);
        ToggleCommand = ReactiveCommand.Create(toggleChange);

    }



    public void toggleChange()
    {
        Console.WriteLine("toggle");
    }



}
