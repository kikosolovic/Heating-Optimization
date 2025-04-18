using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heating_Optimization.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;

namespace Heating_Optimization;

public partial class MainViewModel : ObservableObject
{
    public DateTime MinDateRange1 => new DateTime(2024, 03, 01);
    public DateTime MaxDateRange1 => new DateTime(2024, 03, 14);

    public DateTime MinDateRange2 => new DateTime(2024, 08, 11);
    public DateTime MaxDateRange2 => new DateTime(2024, 08, 24);

    [ObservableProperty]
    private string winterTime = "Winter";

    [ObservableProperty]
    private string summerTime = "Summer";

    public void CONSOLA()
    {
        Console.WriteLine("SI");
    }

    [ObservableProperty]
    private HashSet<int> selectedMachines = new();

    [ObservableProperty]
    private bool isChecked1;
    partial void OnIsChecked1Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((0, value));
        GenerateCsvOnChange();
    }

    [ObservableProperty]
    private bool isChecked2;
    partial void OnIsChecked2Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((1, value));
        GenerateCsvOnChange();
    }

    [ObservableProperty]
    private bool isChecked3;
    partial void OnIsChecked3Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((2, value));
        GenerateCsvOnChange();
    }

    [ObservableProperty]
    private bool isChecked4;
    partial void OnIsChecked4Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((3, value));
        GenerateCsvOnChange();
    }

    [ObservableProperty]
    private bool isChecked5;
    partial void OnIsChecked5Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((4, value));
        GenerateCsvOnChange();
    }

    [RelayCommand]
    private void ToggleMachineSelection((int id, bool isSelected) param)
    {
        if (param.isSelected)
            SelectedMachines.Add(param.id);
        else
            SelectedMachines.Remove(param.id);

        // Notificar cambio del HashSet (por si estás bindeando a otra cosa)
        OnPropertyChanged(nameof(SelectedMachines));
    }
    private readonly OPT _optimizer;

public MainViewModel(OPT optimizer)
{
    _optimizer = optimizer;
}
    

    [RelayCommand]
private void GenerateCsvOnChange()
    {
        string fileName = $"ProductionCost_Selected.csv";

        if (SelectedMachines.Count == 0)
        {
            if (File.Exists(fileName))
        {
            File.Delete(fileName);
            Console.WriteLine($"CSV file {fileName} was deleted because no machines were selected.");
        }
        else
        {
            Console.WriteLine("No machines selected.");
        }
        return;
        }

        
        _optimizer.GenerateCSVForSelectedMachines(SelectedMachines, fileName);

        Console.WriteLine($"CSV successfully generated: {fileName}");
    }

}