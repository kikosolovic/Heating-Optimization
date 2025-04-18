using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heating_Optimization.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Collections.ObjectModel;
using System.Linq;

namespace Heating_Optimization;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
private string selectedScenario;

partial void OnSelectedScenarioChanged(string value)
{
    switch (value)
    {
        case "Escenario 1":
            IsChecked1 = true;
            IsChecked2 = true;
            IsChecked3 = true;
            IsChecked4 = false;
            IsChecked5 = false;
            break;

        case "Escenario 2":
            IsChecked1 = true;
            IsChecked2 = false;
            IsChecked3 = true;
            IsChecked4 = true;
            IsChecked5 = true;
            break;

        case "Own Machines":
            break;
    }
}
private void UpdateScenario()
{
    // Si coincide con Escenario 1
    if (IsChecked1 && IsChecked2 && IsChecked3 && !IsChecked4 && !IsChecked5)
    {
        SelectedScenario = "Escenario 1";
    }
    // Si coincide con Escenario 2
    else if (IsChecked1 && !IsChecked2 && IsChecked3 && IsChecked4 && IsChecked5)
    {
        SelectedScenario = "Escenario 2";
    }
    // Si no coincide con ninguno, Own Machines
    else
    {
        SelectedScenario = "Own Machines";
    }
}
public ObservableCollection<string> ScenarioOptions { get; } = new()
{
    "Escenario 1",
    "Escenario 2",
    "Own Machines"
};
    public ISeries[] Series { get; set; } = [
        new LineSeries<double>
        {
            Values = [2, 1, 3, 5, 3, 4, 6],
            Fill = null,
            GeometrySize = 20
        },
        new LineSeries<int, StarGeometry>
        {
            Values = [4, 2, 5, 2, 4, 5, 3],
            Fill = null,
            GeometrySize = 20,
        }
    ];

    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15)
        };


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
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked2;
    partial void OnIsChecked2Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((1, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked3;
    partial void OnIsChecked3Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((2, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked4;
    partial void OnIsChecked4Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((3, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked5;
    partial void OnIsChecked5Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((4, value));
        UpdateScenario();
        UpdateChart();
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
        SelectedScenario = "Escenario 1";
        AllData = LoadCsv("ProductionCost_Selected.csv");

        var uniqueDates = AllData.Select(d => d.Day).Distinct();
        foreach (var date in uniqueDates)
            AvailableDates.Add(date);

        SelectedDate = AvailableDates.FirstOrDefault();
        SelectedMetric = AvailableMetrics.FirstOrDefault();

        UpdateChart();
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



    [ObservableProperty]
    private List<Axis> yAxis = new()
{

    new Axis {Name = "Values"}
};

    [ObservableProperty]
    private List<Axis> xAxis = new();
    public ObservableCollection<string> AvailableDates { get; } = new();
    public ObservableCollection<string> AvailableMetrics { get; } =
        new() { "Total CO²", "Total Cost", "Percentage of PU Used" };

    [ObservableProperty]
    private string selectedDate;

    [ObservableProperty]
    private string selectedMetric;

    [ObservableProperty]
    private ObservableCollection<ISeries> seriesCollection = new();

    [ObservableProperty]
    private List<string> labels;

    public List<ProductionData> AllData { get; set; }

   


    partial void OnSelectedDateChanged(string value) => UpdateChart();
    partial void OnSelectedMetricChanged(string value) => UpdateChart();

    private void UpdateChart()
    {
        GenerateCsvOnChange();
        AllData = LoadCsv("ProductionCost_Selected.csv");
        if (SelectedDate == null || SelectedMetric == null) return;

        var dataForDate = AllData.Where(d => d.Day == SelectedDate).ToList();
        var names = dataForDate.Select(d => d.Name).Distinct();

        SeriesCollection.Clear();

        foreach (var name in names)
        {
            var dataPoints = dataForDate.Where(d => d.Name == name)
                                        .OrderBy(d => d.Date)
                                        .Select(d => SelectedMetric switch
                                        {
                                            "Total CO²" => d.TotalCO2,
                                            "Total Cost" => d.TotalCost,
                                            "Percentage of PU Used" => d.PercentageUsed,
                                            _ => 0
                                        }).ToList();

            SeriesCollection.Add(new LineSeries<double>
            {
                Name = name,
                Values = new ObservableCollection<double>(dataPoints)
            });
        }

        YAxis.Clear();
        YAxis.Add(new Axis
        {
            Name = SelectedMetric
        });

        XAxis.Clear();
        XAxis.Add(new Axis
        {
            Name = "Time",
            Labels = dataForDate.OrderBy(d => d.Date)
                                .Select(d => d.Hour)
                                .Distinct()
                                .ToList()
        });
    }

    private List<ProductionData> LoadCsv(string path)
    {
        if (!File.Exists(path)) return new List<ProductionData>();

        var lines = File.ReadAllLines(path).Skip(1);
        var data = new List<ProductionData>();

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            data.Add(new ProductionData
            {
                Date = DateTime.ParseExact(parts[0], "dd-MM-yyyy HH:mm", null),
                Period = parts[1],
                Id = int.Parse(parts[2]),
                Name = parts[3],
                TotalHeat = double.Parse(parts[4]),
                PercentageUsed = double.Parse(parts[5]),
                TotalCO2 = double.Parse(parts[6]),
                TotalCost = double.Parse(parts[7])
            });
        }
        return data;
    }

}